using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EggPatrolState", menuName = "AI/Egg/Patrol", order = 1)]
public class EggPatrolState : AIState{
    public float patrolDistance = 5f;
    public float patrolCloseDistance = 0.5f;

    public float detectRange = 10f;
    public float detectInterval = 1f;
    public float detectIntervalVariance = 0.3f;
    public LayerMask detectableLayers;
    public string detectedTag = "Player";

    public float patrolBaseMovespeed = 3f;

    public override void OnEnter(AIDriver driver){
        // False = return to original position
        // True = go to target position
        driver.blackboard.genericBools["patrolState"] = false;
        driver.blackboard.genericTime = detectInterval + Random.Range(-detectIntervalVariance, detectIntervalVariance);
        driver.characterDriver.baseMoveSpeed = patrolBaseMovespeed;
    }

    public override void OnUpdate(AIDriver driver){
        // False = return to original position
        // True = go to target position
        Vector3 moveToLocation;
        if(driver.blackboard.genericBools["patrolState"] == false){
            moveToLocation = driver.blackboard.originalLocation;
        }else{
            moveToLocation = driver.blackboard.desiredLocation;
        }

        MoveTowardsLocation(driver, moveToLocation);

        if(IsCloseToLocation(driver, moveToLocation)){
            driver.blackboard.genericBools["patrolState"] = !driver.blackboard.genericBools["patrolState"];
            if(driver.blackboard.genericBools["patrolState"]){
                driver.blackboard.desiredLocation = GetNewPatrolDestination(driver);
            }
        }

        driver.blackboard.genericTime -= Time.deltaTime;
        if(driver.blackboard.genericTime <= 0f){
            driver.blackboard.genericTime = detectInterval + Random.Range(-detectIntervalVariance, detectIntervalVariance);
            driver.blackboard.targetEnemy = SweepForEnemyDetection(driver, detectedTag);
        }

    }

    public override void OnExit(AIDriver driver){

    }



    public override bool ShouldEnter(AIDriver driver){
        return driver.blackboard.targetEnemy == null;
    }

    public override bool ShouldExit(AIDriver driver){
        return driver.blackboard.targetEnemy != null;
    }

    private void MoveTowardsLocation(AIDriver driver, Vector3 location){
        Vector3 direction3D = (location - driver.rb.position);
        Vector2 direction2D = new Vector2(direction3D.x, direction3D.z).normalized;
        driver.characterDriver.OnMove(direction2D);
    }

    private bool IsCloseToLocation(AIDriver driver, Vector3 location){
        Vector2 location2D = new Vector2(location.x, location.z);
        Vector2 characterLoc2D = new Vector2(driver.rb.position.x, driver.rb.position.z);
        return (location2D - characterLoc2D).magnitude <= patrolCloseDistance;
    }

    private Vector3 GetNewPatrolDestination(AIDriver driver){
        float rand = Random.Range(0f, 2f * Mathf.PI);
        Vector3 locationAdd = new Vector3(Mathf.Cos(rand), 0f, Mathf.Sin(rand)) * patrolDistance;
        return driver.blackboard.originalLocation + locationAdd;
    }

    private Transform SweepForEnemyDetection(AIDriver driver, string tag){
        Collider[] hits = Physics.OverlapSphere(driver.rb.position, detectRange, detectableLayers);
        foreach(Collider hit in hits){
            if(hit.tag == tag){
                return hit.GetComponentInParent<Rigidbody>().transform;
            }
        }

        return null;
    }

}
