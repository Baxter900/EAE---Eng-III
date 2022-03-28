using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "JumpState", menuName = "States/Jump", order = 1)]
public class JumpState : CharacterControllerState{

    [Header("Jumping")]

    [SerializeField]
    [Tooltip("This is the impulse jump force we apply.")]
    protected float jumpImpulseForce = 100f;

    [SerializeField]
    [Tooltip("This is the constant upwards force we apply while holding the jump key.")]
    protected float jumpConstantForce = 10f;

    [SerializeField]
    [Tooltip("This is how long (in seconds) we can hold the jump key for before we stop jumping.")]
    protected float jumpDuration = 0.2f;

    [SerializeField]
    [Tooltip("This is how long (in seconds) after becoming airborne we can still jump.")]
    protected float coyoteTime = 0.1f;

    [SerializeField]
    [Tooltip("This is the move speed we use while jumping.")]
    protected float moveSpeedWhileJumping = 2f;



    public override void OnEnter(CharacterControllerDriver driver){

    }

    protected virtual bool CanJump(CharacterControllerDriver driver){
        // If we're not grounded, we can still jump if we have remaining coyoteTime AND we could be grounded if we touched the ground.
        return driver.moveState.isGrounded || (driver.moveState.unGroundedCount < coyoteTime && driver.moveState.preventGroundedCount <= 0f);
    }

    public override void OnFixedUpdate(CharacterControllerDriver driver){
        driver.rb.AddForce(moveSpeedWhileJumping * driver.moveState.worldInput, ForceMode.Acceleration);

        if(driver.moveState.shouldAttemptJump && CanJump(driver)){
            driver.moveState.shouldAttemptJump = false;
            driver.ProcessJumpVariables();
            driver.rb.AddForce(jumpImpulseForce * driver.moveState.UpVector, ForceMode.VelocityChange);
        }
        if(driver.moveState.isJumpButtonDown && driver.moveState.jumpingCount <= jumpDuration && !driver.moveState.isGrounded){
            driver.rb.AddForce(driver.moveState.UpVector * jumpConstantForce, ForceMode.Acceleration);
        }
    }

    public override void OnExit(CharacterControllerDriver driver){

    }


    public override bool ShouldEnter(CharacterControllerDriver driver){
        return CanJump(driver)
            && driver.moveState.isJumpButtonDown
            && driver.moveState.shouldAttemptJump;
    }

    public override bool ShouldExit(CharacterControllerDriver driver){
        return !driver.moveState.isJumpButtonDown
            || driver.moveState.jumpingCount > jumpDuration;
    }

}
