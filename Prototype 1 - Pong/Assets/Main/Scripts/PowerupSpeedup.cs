using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupSpeedup : Powerup{

    [SerializeField]
    private float speedMult = 2f;

    public override void DoEffect(BallController ball){
        ball.rb.velocity *= speedMult;
    }
}
