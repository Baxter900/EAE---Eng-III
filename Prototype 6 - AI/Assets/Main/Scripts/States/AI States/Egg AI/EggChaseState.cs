using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EggChaseState", menuName = "AI/Egg/Chase", order = 1)]
public class EggChaseState : AIState{
    public float detectRange = 10f;
    public float detectInterval = 0.1f;
    public float detectIntervalVariance = 0f;
    public LayerMask detectableLayers;
    public string detectedTag = "Player";

    public float rangeToExplodeAt = 0.3f;

    public float chaseBaseMoveSpeed = 11f;

    public bool canLeaveChase = true;

    public override void OnEnter(AIDriver driver){
        driver.characterDriver.baseMoveSpeed = chaseBaseMoveSpeed;
    }

    public override void OnUpdate(AIDriver driver){
        if(driver.blackboard.targetEnemy == null){
            return;
        }

        // False = return to original position
        // True = go to target position
        Vector3 moveToLocation = driver.blackboard.targetEnemy.position;

        MoveTowardsLocation(driver, moveToLocation);

        if(IsCloseToLocation(driver, moveToLocation)){
            // Explode
            driver.blackboard.genericBools["shouldExplode"] = true;
            Debug.Log("boom");
        }

        if(canLeaveChase){
            driver.blackboard.genericTime -= Time.deltaTime;
            if(driver.blackboard.genericTime <= 0f){
                driver.blackboard.genericTime = detectInterval + Random.Range(-detectIntervalVariance, detectIntervalVariance);
                driver.blackboard.targetEnemy = SweepForEnemyDetection(driver, detectedTag);
            }
        }

    }

    public override void OnExit(AIDriver driver){

    }



    public override bool ShouldEnter(AIDriver driver){
        bool isSetup = driver.blackboard.genericBools.TryGetValue("shouldExplode", out bool shouldExplode);
        if(!isSetup){
            shouldExplode = false;
        }
        return driver.blackboard.targetEnemy != null && !shouldExplode;
    }

    public override bool ShouldExit(AIDriver driver){
        bool isSetup = driver.blackboard.genericBools.TryGetValue("shouldExplode", out bool shouldExplode);
        if(!isSetup){
            shouldExplode = false;
        }
        return driver.blackboard.targetEnemy == null || shouldExplode;
    }

    private void MoveTowardsLocation(AIDriver driver, Vector3 location){
        Vector3 direction3D = (location - driver.rb.position);
        Vector2 direction2D = new Vector2(direction3D.x, direction3D.z).normalized;
        driver.characterDriver.OnMove(direction2D);
    }

    private bool IsCloseToLocation(AIDriver driver, Vector3 location){
        Vector2 location2D = new Vector2(location.x, location.z);
        Vector2 characterLoc2D = new Vector2(driver.rb.position.x, driver.rb.position.z);
        return (location2D - characterLoc2D).magnitude <= rangeToExplodeAt;
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
