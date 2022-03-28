using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Collider2D))][RequireComponent(typeof(Rigidbody2D))]
public class PaddleController : MonoBehaviour{

    [SerializeField]
    [Tooltip("The speed the paddle moves at.")]
    private float speed;

    [SerializeField]
    [Tooltip("The size of the paddle.")]
    private float _paddleSize;

    [SerializeField]
    [Tooltip("The position the ball will spawn at on the paddle.")]
    public Transform ballSpawnPoint;

    // Variable tracking the current direction being held
    private float inputDir = 0f;


    public float PaddleSize{
        get{
            return _paddleSize;
        }
        set{
            _paddleSize = value;
            Vector3 tempScale = transform.localScale;
            tempScale.y = _paddleSize;
            transform.localScale = tempScale;
        }
    }


    private Collider2D coll;
    private Rigidbody2D rb;


    void Start(){
        coll = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        PaddleSize = _paddleSize;
    }

    void FixedUpdate(){
        Vector2 tempVel = rb.velocity;
        tempVel.y = inputDir * speed;
        rb.velocity = tempVel;
    }

    public void OnMoveInput(InputAction.CallbackContext context){
        if(context.performed){
            inputDir = context.ReadValue<float>();
        }else{
            inputDir = 0f;
        }
        // Debug.Log(inputDir);
    }

}
