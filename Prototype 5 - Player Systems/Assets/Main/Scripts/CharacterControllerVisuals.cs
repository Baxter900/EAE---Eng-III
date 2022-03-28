using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControllerVisuals : MonoBehaviour{
    public CharacterControllerDriver driver;
    public Animator animator;
    public Transform visualsRotateObject;
    public float rotateTowardsFactor = 6f;

    void Update(){
        Vector3 projectedVel = driver.rb.velocity;
        // projectedVel = Vector3.ProjectOnPlane(projectedVel, driver.moveState.UpVector);
        if(projectedVel.magnitude > 0.01f){
            Quaternion targetRot = Quaternion.LookRotation(projectedVel, driver.moveState.UpVector);
            visualsRotateObject.rotation = Quaternion.Slerp(visualsRotateObject.rotation, targetRot, rotateTowardsFactor * Time.deltaTime);
        }

        animator.SetFloat("Speed", driver.rb.velocity.magnitude);
        animator.SetBool("IsWalk", driver.activeState == driver.walkState || driver.activeState == driver.fallbackState);
        animator.SetBool("IsDodge", driver.activeState == driver.dodgeState);
        animator.SetBool("IsJump", driver.activeState == driver.jumpState);
        animator.SetBool("IsFall", driver.activeState == driver.fallingState);
        animator.SetBool("IsGlide", driver.activeState == driver.glideState);
    }
}
