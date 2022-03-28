using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class GameManager : MonoBehaviour{
    private static GameManager _instance = null;

    public static GameManager Instance{
        get{
            return _instance;
        }

        set{
            if(_instance == null){
                _instance = value;
            }else{
                throw new UnityException("Tried to create a second instance of GameManager. There can only be one instance of GameManager at a time.");
            }
        }
    }

    void Awake(){
        _instance = this;
    }



    [Header("References")]

    [SerializeField]
    [Tooltip("The player 1 Paddle.")]
    private PaddleController player1;

    [SerializeField]
    [Tooltip("The player 2 Paddle.")]
    private PaddleController player2;

    [SerializeField]
    [Tooltip("The ball prefab.")]
    private GameObject ballPrefab;


    [Header("Ball")]

    [SerializeField]
    [Tooltip("The speed the ball spawns at.")]
    private float initialBallSpeed = 3f;

    [SerializeField]
    [Tooltip("How long it takes the ball to spawn.")]
    private float ballSpawnDuration = 2f;

    [Header("Scoring")]

    [SerializeField]
    [Tooltip("The point increase when scoring.")]
    private int pointIncreasePerScore = 1;




    private Transform spawnedBall = null;
    private float spawnTimeRemaining;
    private bool isBallSpawning = false;

    private int lastLoserID = 0;

    [HideInInspector]
    public int pointIncreaseForNextScore = 0;


    void Start(){
        pointIncreaseForNextScore = pointIncreasePerScore;
        spawnedBall = GameObject.Instantiate(ballPrefab).transform;
        SpawnBall();
    }

    public void ScorePoint(int losingPlayerID){
        Assert.IsTrue(losingPlayerID == 0 || losingPlayerID == 1, $"Expected a losingPlayerID of 0 or 1, got {losingPlayerID}");
        ScoreManager.Instance.AddToScore(ScoreManager.GetWinnerIDFromLoserID(losingPlayerID), pointIncreaseForNextScore);
        pointIncreaseForNextScore = pointIncreasePerScore;
        lastLoserID = losingPlayerID;
        spawnedBall.gameObject.SetActive(false);
        SpawnBall();
    }

    private void SpawnBall(){
        StartCoroutine(SpawnBallCoroutine());
    }

    private IEnumerator SpawnBallCoroutine(){
        // Prevent us from making 2 of these coroutines at a time.
        if(isBallSpawning){
            yield break;
        }
        isBallSpawning = true;
        spawnTimeRemaining = ballSpawnDuration;
        spawnedBall.gameObject.SetActive(true);
        spawnedBall.localScale = Vector3.one;

        while(spawnTimeRemaining > 0f){
            spawnedBall.position = GetBallSpawnPosition();
            yield return null;
            spawnTimeRemaining -= Time.deltaTime;
        }

        // Set velocity to aim towards the center
        spawnedBall.GetComponentInChildren<Rigidbody2D>().velocity = new Vector2(initialBallSpeed * -Mathf.Sign(spawnedBall.position.x), 0f);
        SFXManager.Instance.PlayPaddleBounceSound();
        isBallSpawning = false;
    }

    private Vector3 GetBallSpawnPosition(){
        Assert.IsTrue(lastLoserID == 0 || lastLoserID == 1, $"Expected a lastLoserID of 0 or 1, got {lastLoserID}");
        if(lastLoserID == 0){
            return player1.ballSpawnPoint.position;
        }else{
            return player2.ballSpawnPoint.position;
        }
    }

}
