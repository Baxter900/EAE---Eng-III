using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FallingState", menuName = "States/Falling", order = 1)]
public class FallingState : CharacterControllerState{

    public float moveSpeedWhileFallingMult;

    public override void OnEnter(CharacterControllerDriver driver){

    }

    public override void OnFixedUpdate(CharacterControllerDriver driver){
        driver.rb.AddForce(driver.baseMoveSpeed * moveSpeedWhileFallingMult * driver.moveState.worldInput, ForceMode.Acceleration);
    }

    public override void OnExit(CharacterControllerDriver driver){

    }


    public override bool ShouldEnter(CharacterControllerDriver driver){
        return !driver.moveState.isGrounded
            && !driver.moveState.shouldAttemptJump
            && !driver.moveState.isJumpButtonDown;
    }

    public override bool ShouldExit(CharacterControllerDriver driver){
        return driver.moveState.isGrounded
            || driver.moveState.shouldAttemptJump
            || driver.moveState.isJumpButtonDown;
    }

}
