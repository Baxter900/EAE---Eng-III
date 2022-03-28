using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup2x : Powerup{
    public override void DoEffect(BallController ball){
        ball.transform.localScale *= 2f;
        GameManager.Instance.pointIncreaseForNextScore *= 2;
    }
}
