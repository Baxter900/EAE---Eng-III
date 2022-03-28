using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupVortex : Powerup{

    [SerializeField]
    private float minReleaseSpeed = 3f;

    protected override void Start(){
        base.Start();

    }

    public override void DoEffect(BallController ball){
        if(ball.rb.velocity.magnitude < 3f){
            ball.rb.velocity = ball.rb.velocity.normalized * minReleaseSpeed;
        }
    }
}
