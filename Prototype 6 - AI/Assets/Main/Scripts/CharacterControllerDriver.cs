using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[System.Serializable]
public class MovementState{

    // ### Input ###

    // This is true if the jump input is currently down
    public bool isJumpButtonDown = false;
    // This is the raw vector of our directional input
    public Vector2 rawInput = Vector2.zero;
    // If this is true, we should try and jump next FixedUpdate
    public bool shouldAttemptJump = false;
    // If this is true, we should try and dodge next FixedUpdate
    public bool shouldAttemptDodge = false;

    // ### Directions ###

    // We define this as a constant in case we ever need to support nonstandard up directions
    public Vector3 UpVector = Vector3.up;

    // These are the camera's forward and right vector projected onto the flat input plane
    public Vector3 camForwardOnPlane = Vector3.zero;
    public Vector3 camRightOnPlane = Vector3.zero;

    // This is the vector of our input direction in world space
    public Vector3 worldInput = Vector3.zero;

    // ### State ###

    public float jumpingCount = 0f;

    public bool isGrounded = false;
    public float unGroundedCount = 0f;
    public float groundedCount = 0f;
    public float preventGroundedCount = 0f;
    public Vector3 groundNormal = Vector3.up;
    public float groundDotUp = 1f;
    public float groundAngle = 0f;

    public bool isDodging = false;
    public float dodgingCount = 0f;
    public Vector3 worldDodgeDir = Vector3.zero;

    public Rigidbody standingOnObject = null;
    public Vector3 movingObjectVelocity = Vector3.zero;
    public Vector3 lastMovingObjectVelocity = Vector3.zero;

}

[System.Serializable]
public class AttackState{

    public float genericT;

    public Dictionary<string, GameObject> managedObjs;
    public Dictionary<string, Component> managedComps;
}

public class CharacterControllerDriver : MonoBehaviour{

    [SerializeField][Required]
    public CharacterControllerState walkState;
    [SerializeField][Required]
    public CharacterControllerState jumpState;
    [SerializeField][Required]
    public CharacterControllerState fallingState;
    [SerializeField][Required]
    public CharacterControllerState dodgeState;
    [SerializeField][Required]
    public CharacterControllerState glideState;
    [SerializeField][Required]
    public CharacterControllerState fallbackState;

    private CharacterControllerState[] allStates;

    [SerializeField][Required]
    [Tooltip("This is the transform we treat as the camera for purposes of determining input direction.")]
    public Transform cameraTransform;

    [SerializeField][Required]
    [Tooltip("This is the rigidbody we affect as the character.")]
    public Rigidbody rb;

    [SerializeField]
    [Tooltip("This is the attack inventory of the character.")]
    public CharacterControllerAttacks attacks;

    [SerializeField]
    [Tooltip("The base move speed, all other movement is a multiplier of this.")]
    public float baseMoveSpeed = 10f;

    [Header("Grounding")]

    [SerializeField]
    [Tooltip("These are the layers we treat as part of the ground.")]
    private LayerMask groundCheckLayerMask;

    [SerializeField][Required]
    [Tooltip("This is where we start the raycast to check the ground. We expect this to be at least groundCheckRadius from the ground at all times and no farther than groundCheckDistance from the ground when touching it.")]
    private Transform groundCheckOrigin;

    [SerializeField]
    [Tooltip("This is the distance that we check for the ground from the groundCheckOrigin. This should generally be slightly more than the distance of groundCheckOrigin to the ground when standing upright.")]
    private float groundCheckDistance;

    [SerializeField]
    [Tooltip("This is the radius of our Spherecast to the ground.")]
    private float groundCheckRadius;

    [SerializeField]
    [Tooltip("This is the time we continue to pay attention to the ground below us after becoming ungrounded.")]
    private float keepOldGroundTime;

    [SerializeField]
    [Tooltip("How long we want to prevent the character from being grounded after jumping.")]
    private float preventGroundingAfterJumpingTime = 0.05f;

    [Header("Attacks")]

    [SerializeField]
    [Tooltip("This is the position attacks originate from.")]
    public Transform attackOrigin;


    [SerializeField] [ReadOnly] [FoldoutGroup("Debug")]
    public CharacterControllerState activeState;

    [SerializeField] [ReadOnly] [FoldoutGroup("Debug")]
    public BaseAttack selectedAttack = null;

    [SerializeField] [ReadOnly] [FoldoutGroup("Debug")]
    private bool isAttackPressed = false;

    [SerializeField] [ReadOnly] [FoldoutGroup("Debug")]
    public MovementState moveState;

    [SerializeField] [ReadOnly] [FoldoutGroup("Debug")]
    public AttackState attackState;

    void Start(){
        // The order here determines the order in which the states are prioritized when multiple can be entered
        allStates = new CharacterControllerState[]{
            walkState,
            jumpState,
            fallingState,
            dodgeState,
            glideState,
            fallbackState
        };
        activeState = walkState;

        attackState.managedObjs = new Dictionary<string, GameObject>();
        attackState.managedComps = new Dictionary<string, Component>();
    }

    void Update(){
        UpdateCounts();
    }

    void FixedUpdate(){
        if(cameraTransform){
            // Update the movestate vectors
            moveState.camForwardOnPlane = Vector3.ProjectOnPlane(cameraTransform.forward, moveState.UpVector).normalized;
            moveState.camRightOnPlane = Vector3.ProjectOnPlane(cameraTransform.right, moveState.UpVector).normalized;
            moveState.worldInput = moveState.rawInput.x * moveState.camRightOnPlane + moveState.rawInput.y * moveState.camForwardOnPlane;

            FixedUpdateGrounded();
            CheckTransitionStates();

            activeState.OnFixedUpdate(this);

            if(isAttackPressed && !activeState.doesAllowAttacks){
                ChangeIsAttackPressed(false);
            }

            if(isAttackPressed && selectedAttack != null){
                selectedAttack.OnFixedUpdate(this);
            }
        }
    }

    private void CheckTransitionStates(){
        if(activeState.ShouldExit(this)){
            for(int i = 0; i < allStates.Length; i++){
                if(allStates[i].ShouldEnter(this)){
                    ChangeState(allStates[i]);
                    return;
                }
            }
        }
    }

    private void ChangeState(CharacterControllerState newState){
        if(newState == activeState){
            return;
        }

        activeState.OnExit(this);
        activeState = newState;
        activeState.OnEnter(this);
    }

    public void ChangeAttack(BaseAttack newAttack){
        if(newAttack == selectedAttack){
            return;
        }

        if(selectedAttack != null){
            if(isAttackPressed){
                selectedAttack.OnButtonPressEnd(this);
            }
            selectedAttack.OnDeselected(this);
        }
        selectedAttack = newAttack;
        if(selectedAttack != null){
            selectedAttack.OnSelected(this);
            if(isAttackPressed){
                selectedAttack.OnButtonPressStart(this);
            }
        }
    }

    private void ChangeIsAttackPressed(bool newValue){
        if(isAttackPressed != newValue){
            isAttackPressed = newValue;
            if(selectedAttack != null){
                if(newValue){
                    selectedAttack.OnButtonPressStart(this);
                }else{
                    selectedAttack.OnButtonPressEnd(this);
                }
            }
        }
    }

    private void FixedUpdateGrounded(){
        RaycastHit? rayHit = null;
        RaycastHit hitInfo;

        if (Physics.SphereCast(groundCheckOrigin.position, groundCheckRadius, -moveState.UpVector, out hitInfo, groundCheckDistance, groundCheckLayerMask)){
            rayHit = hitInfo;
        }

        if(rayHit.HasValue){
            // Update basic values
            moveState.unGroundedCount = 0f;
            moveState.standingOnObject = rayHit.Value.rigidbody;
            // Track some values for future frames
            moveState.groundNormal = rayHit.Value.normal;
            moveState.groundDotUp = Vector3.Dot(rb.transform.up, moveState.groundNormal);
            // Calculate the groundAngle (for animation purposes)
            float forwardDotNormal = Vector3.Dot(rb.transform.forward, rayHit.Value.normal);
            float groundAngleUpComponent = Vector3.Angle(rb.transform.up, rayHit.Value.normal);
            if(forwardDotNormal > 0f){
                groundAngleUpComponent *= -1f;
            }
            moveState.groundAngle = groundAngleUpComponent * Mathf.Abs(forwardDotNormal);

            if(moveState.standingOnObject){
                moveState.movingObjectVelocity = moveState.standingOnObject.GetPointVelocity(rayHit.Value.point);
				moveState.movingObjectVelocity.y = 0f;
            }else{
				moveState.movingObjectVelocity = Vector3.zero;
            }
            // Increase grounded count
            moveState.groundedCount += Time.fixedDeltaTime;

            moveState.isGrounded = moveState.preventGroundedCount <= 0f;
            if(moveState.isGrounded){
                moveState.jumpingCount = Mathf.Infinity;
            }
        }else{
            // Set ground states in their ungrounded state
            moveState.groundNormal = Vector3.up;
            moveState.groundDotUp = 1f;
            moveState.unGroundedCount += Time.fixedDeltaTime;

            // Stop holding this object if we've been ungrounded long enough
            if (moveState.unGroundedCount > keepOldGroundTime){
    			moveState.standingOnObject = null;
    		}

            // Set other grounded related states to their empty values
            moveState.movingObjectVelocity = Vector3.zero;
    		moveState.groundedCount = 0f;
            moveState.isGrounded = false;
        }
    }

    private void UpdateCounts(){
        // Grounded counts are updated in FixedUpdateGrounded instead
        moveState.jumpingCount += Time.deltaTime;
        moveState.preventGroundedCount -= Time.deltaTime;
        moveState.dodgingCount += Time.deltaTime;
    }

    public void ProcessJumpVariables(){
        moveState.jumpingCount = 0f;
        moveState.isGrounded = false;
        moveState.preventGroundedCount = preventGroundingAfterJumpingTime;
    }

    public void OnMove(Vector2 inputDir){
        moveState.rawInput = inputDir;
    }

    public void OnJump(bool isButtonDown){
        moveState.isJumpButtonDown = isButtonDown;
        // if(!isButtonDown){
        //     moveState.jumpingCount = Mathf.Infinity;
        // }
        if(isButtonDown){
            moveState.shouldAttemptJump = true;
        }
    }

    public void OnDodge(bool isButtonDown){
        if(isButtonDown && moveState.isGrounded){
            moveState.shouldAttemptDodge = true;
        }
    }

    public void OnAttack(bool isButtonDown){
        ChangeIsAttackPressed(isButtonDown);
    }
}
