using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WalkState", menuName = "States/Walk", order = 1)]
public class WalkState : CharacterControllerState{

    public float moveSpeed;
    public float dragFactor;

    public override void OnEnter(CharacterControllerDriver driver){

    }

    public override void OnFixedUpdate(CharacterControllerDriver driver){
        driver.rb.AddForce(moveSpeed * driver.moveState.worldInput, ForceMode.Acceleration);

        Vector3 nonForwardVelocity = driver.rb.velocity - Vector3.Project(driver.rb.velocity, driver.moveState.worldInput);
        Vector3 nonForwardHorizontalVelocity = nonForwardVelocity;
        nonForwardHorizontalVelocity.y = 0f;

        driver.rb.AddForce(-nonForwardHorizontalVelocity * dragFactor, ForceMode.Acceleration);
    }

    public override void OnExit(CharacterControllerDriver driver){

    }


    public override bool ShouldEnter(CharacterControllerDriver driver){
        return driver.moveState.isGrounded
            && !driver.moveState.shouldAttemptJump
            && !driver.moveState.shouldAttemptDodge;
    }

    public override bool ShouldExit(CharacterControllerDriver driver){
        return !driver.moveState.isGrounded
            || driver.moveState.shouldAttemptJump
            || driver.moveState.shouldAttemptDodge;
    }

}
