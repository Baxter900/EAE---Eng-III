using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BallController : MonoBehaviour{

    [SerializeField]
    [Tooltip("The angle away from the paddle normal that the ball bounces at when at the very edge of the paddle.")]
    private float maxPaddleBounceAngle = 60f;

    [HideInInspector]
    public Rigidbody2D rb;

    void Start(){
        rb = GetComponent<Rigidbody2D>();
    }

    void OnTriggerEnter2D(Collider2D other){
        DeathZone deathZone = other.GetComponent<DeathZone>();
        if(deathZone){
            GameManager.Instance.ScorePoint(deathZone.isPlayer1DeathZone ? 0 : 1);
            SFXManager.Instance.PlayScoreBounceSound();
        }
    }

    void OnCollisionEnter2D(Collision2D coll){
        PaddleController paddle = coll.gameObject.GetComponent<PaddleController>();
        if(paddle){
            float yDiffFromPaddleCenter = transform.position.y - paddle.transform.position.y;
            float percentFromCenter = 2f * (yDiffFromPaddleCenter / paddle.PaddleSize);
            // Debug.Log(percentFromCenter);

            float ballSpeed = rb.velocity.magnitude;
            // Set up the ball angle from the bounce
            Vector2 newDir = new Vector2(Mathf.Cos(maxPaddleBounceAngle * Mathf.Deg2Rad * percentFromCenter), Mathf.Sin(maxPaddleBounceAngle * Mathf.Deg2Rad * percentFromCenter));
            // Make sure the ball is still heading towards the center
            newDir.x = Mathf.Abs(newDir.x) * -Mathf.Sign(transform.position.x);
            rb.velocity = newDir * ballSpeed;

            SFXManager.Instance.PlayPaddleBounceSound();
        }else{
            SFXManager.Instance.PlayWallBounceSound();
        }
    }
}
