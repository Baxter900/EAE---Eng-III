using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Assertions;

public class ScoreManager : MonoBehaviour{
    private static ScoreManager _instance = null;

    public static ScoreManager Instance{
        get{
            return _instance;
        }

        set{
            if(_instance == null){
                _instance = value;
            }else{
                throw new UnityException("Tried to create a second instance of ScoreManager. There can only be one instance of ScoreManager at a time.");
            }
        }
    }

    void Awake(){
        _instance = this;
    }


    [SerializeField]
    [Tooltip("The text object for player 1's score.")]
    private TMP_Text player1ScoreText;

    [SerializeField]
    [Tooltip("The text object for player 2's score.")]
    private TMP_Text player2ScoreText;


    private int player1Score = 0;
    private int player2Score = 0;

    void Start(){
        SetScoreUI(0, player1Score);
        SetScoreUI(1, player2Score);
    }

    public static int GetWinnerIDFromLoserID(int loserID){
        Assert.IsTrue(loserID == 0 || loserID == 1);
        if(loserID == 0){
            return 1;
        }else{
            return 0;
        }
    }

    public void AddToScore(int playerID, int amountToAdd){
        Assert.IsTrue(playerID == 0 || playerID == 1);

        if(playerID == 0){
            player1Score += amountToAdd;
            SetScoreUI(playerID, player1Score);
        }else{
            player2Score += amountToAdd;
            SetScoreUI(playerID, player2Score);
        }
    }

    private void SetScoreUI(int playerID, int newScore){
        Assert.IsTrue(playerID == 0 || playerID == 1);

        if(playerID == 0){
            // This is one of the 9000 different ways to convert an int to a string with exactly 2 digits.
            player1ScoreText.text = newScore.ToString("00");
        }else{
            // This is one of the 9000 different ways to convert an int to a string with exactly 2 digits.
            player2ScoreText.text = newScore.ToString("00");
        }
    }

}
