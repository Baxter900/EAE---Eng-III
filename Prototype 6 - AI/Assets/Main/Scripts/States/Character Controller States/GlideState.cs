using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GlideState", menuName = "States/Glide", order = 1)]
public class GlideState : CharacterControllerState{
    public float glideForce;
    public float moveSpeedWhileGlidingMultiplier;
    public float dragFactor;

    public override void OnEnter(CharacterControllerDriver driver){

    }

    public override void OnFixedUpdate(CharacterControllerDriver driver){
        driver.rb.AddForce(driver.baseMoveSpeed * moveSpeedWhileGlidingMultiplier * driver.moveState.worldInput, ForceMode.Acceleration);
        driver.rb.AddForce(glideForce * driver.moveState.UpVector, ForceMode.Acceleration);

        Vector3 nonForwardVelocity = driver.rb.velocity - Vector3.Project(driver.rb.velocity, driver.moveState.worldInput);
        Vector3 nonForwardHorizontalVelocity = nonForwardVelocity;
        nonForwardHorizontalVelocity.y = 0f;

        driver.rb.AddForce(-nonForwardHorizontalVelocity * dragFactor, ForceMode.Acceleration);
    }

    public override void OnExit(CharacterControllerDriver driver){

    }


    public override bool ShouldEnter(CharacterControllerDriver driver){
        return !driver.moveState.isGrounded
            && !driver.moveState.shouldAttemptJump
            && driver.moveState.isJumpButtonDown
            && driver.rb.velocity.y <= 0f;
    }

    public override bool ShouldExit(CharacterControllerDriver driver){
        return driver.moveState.isGrounded
            || driver.moveState.shouldAttemptJump
            || !driver.moveState.isJumpButtonDown;
    }

}
