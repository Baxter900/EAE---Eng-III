using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "BossIdleState", menuName = "AI/Boss/Idle", order = 1)]
public class BossIdleState : AIState{

    public float detectRange = 10f;
    public float detectInterval = 1f;
    public LayerMask detectableLayers;
    public string detectedTag = "Player";

    public UnityEvent playerDetected = new UnityEvent();

    public override void OnEnter(AIDriver driver){
        driver.blackboard.genericTime = detectInterval;
        driver.blackboard.genericInts["bossStage"] = 0;
    }

    public override void OnUpdate(AIDriver driver){

        driver.blackboard.genericTime -= Time.deltaTime;
        if(driver.blackboard.genericTime <= 0f){
            driver.blackboard.genericTime = detectInterval;
            driver.blackboard.targetEnemy = SweepForEnemyDetection(driver, detectedTag);
            if(driver.blackboard.targetEnemy != null){
                driver.blackboard.genericInts["bossStage"] = 1;
            }
        }

    }

    public override void OnExit(AIDriver driver){
        playerDetected.Invoke();
        driver.blackboard.genericInts["bossStage"] = 1;
        driver.rb.GetComponent<LookAtTransform>().target = driver.blackboard.targetEnemy;
    }



    public override bool ShouldEnter(AIDriver driver){
        return driver.blackboard.targetEnemy == null;
    }

    public override bool ShouldExit(AIDriver driver){
        return driver.blackboard.targetEnemy != null;
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
