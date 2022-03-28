using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public abstract class Powerup : MonoBehaviour{

    private float remainingTimeUntilDespawn = 0f;

    protected virtual void Start(){
        remainingTimeUntilDespawn = PowerupManager.Instance.powerupTimeBeforeDespawn;
    }

    protected virtual void Update(){
        remainingTimeUntilDespawn -= Time.deltaTime;

        if(remainingTimeUntilDespawn <= 0f){
            PowerupManager.Instance.DespawnPowerup(this);
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D other){
        BallController ball = other.GetComponent<BallController>();
        if(ball){

            SFXManager.Instance.PlayerPowerupSound();
            DoEffect(ball);
            PowerupManager.Instance.DespawnPowerup(this);
        }
    }

    public abstract void DoEffect(BallController ball);
}
