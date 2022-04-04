using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DodgeState", menuName = "States/Dodge", order = 1)]
public class DodgeState : CharacterControllerState{

    public float dodgeDuration;
    public float dodgeSpeed;
    public float dodgeImpulseSpeed;
    public float maxExitDodgeSpeed;

    public override void OnEnter(CharacterControllerDriver driver){
         driver.moveState.worldDodgeDir = driver.moveState.worldInput;
         driver.rb.velocity = driver.moveState.worldInput * dodgeImpulseSpeed;
         driver.moveState.shouldAttemptDodge = false;
         driver.moveState.dodgingCount = 0f;
    }

    public override void OnFixedUpdate(CharacterControllerDriver driver){
        driver.rb.AddForce(dodgeSpeed * driver.moveState.worldDodgeDir, ForceMode.Acceleration);
    }

    public override void OnExit(CharacterControllerDriver driver){
         driver.moveState.worldDodgeDir = Vector3.zero;

         if(driver.rb.velocity.magnitude > maxExitDodgeSpeed){
             driver.rb.velocity = driver.rb.velocity.normalized * maxExitDodgeSpeed;
         }
    }


    public override bool ShouldEnter(CharacterControllerDriver driver){
        return driver.moveState.isGrounded
            && !driver.moveState.shouldAttemptJump
            && driver.moveState.shouldAttemptDodge;
    }

    public override bool ShouldExit(CharacterControllerDriver driver){
        return !driver.moveState.isGrounded
            || driver.moveState.shouldAttemptJump
            || driver.moveState.dodgingCount >= dodgeDuration;
    }

}
