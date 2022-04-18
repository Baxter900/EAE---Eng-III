using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "BossSpearAttackStateEnd", menuName = "AI/Boss/SpearAttackEnd", order = 1)]
public class BossSpearAttackStateEnd : AIState{
    public float spearAttackInterval = 0.15f;
    public float spearMoveSpeed = 10f;
    public float timeBefore = 2f;
    public float timeAfter = 3f;
    public int bossStage = 2;


    public override void OnEnter(AIDriver driver){
        driver.blackboard.genericTime = timeBefore;
        driver.blackboard.genericInts["spearAttackEndStage"] = 0;
    }

    public override void OnUpdate(AIDriver driver){
        int remainingSpears = driver.blackboard.genericInts["spawnedSpears"];

        driver.blackboard.genericTime -= Time.deltaTime;

        if(driver.blackboard.genericTime <= 0f){
            if(driver.blackboard.genericInts["spearAttackEndStage"] == 0){
                driver.blackboard.genericTime = spearAttackInterval;
                driver.blackboard.genericInts["spearAttackEndStage"] = 1;
            }else if(driver.blackboard.genericInts["spearAttackEndStage"] == 1){
                if(remainingSpears > 0){
                    driver.blackboard.genericTime = spearAttackInterval;

                    if(driver.blackboard.tempCreatedObjects.Count > 0){
                        GameObject spear = driver.blackboard.tempCreatedObjects[0];
                        driver.blackboard.objectsToDelete.Add(spear);
                        driver.blackboard.tempCreatedObjects.Remove(spear);
                        driver.blackboard.genericInts["spawnedSpears"]--;

                        MoveTowardTransform moveToward = spear.GetComponent<MoveTowardTransform>();
                        moveToward.targetPosition = driver.blackboard.targetEnemy.position;
                        moveToward.speed = spearMoveSpeed;
                        moveToward.useTargetTransform = false;
                        moveToward.isActive = true;

                        LookAtTransform lookAt = spear.GetComponent<LookAtTransform>();
                        lookAt.useTargetTransform = false;
                        lookAt.targetPosition = driver.blackboard.targetEnemy.position;
                    }
                }else{
                    driver.blackboard.genericInts["spearAttackEndStage"] = 2;
                    driver.blackboard.genericTime = timeAfter;
                }
            }
        }
    }

    public override void OnExit(AIDriver driver){
        foreach(GameObject obj in driver.blackboard.tempCreatedObjects){
            Destroy(obj);
        }

        foreach(GameObject obj in driver.blackboard.objectsToDelete){
            Destroy(obj);
        }

        driver.blackboard.objectsToDelete.Clear();
        driver.blackboard.tempCreatedObjects.Clear();
    }



    public override bool ShouldEnter(AIDriver driver){
        if(!driver.blackboard.genericInts.ContainsKey("bossStage")){
            driver.blackboard.genericInts["bossStage"] = 0;
        }

        if(!driver.blackboard.genericInts.ContainsKey("spawnedSpears")){
            driver.blackboard.genericInts["spawnedSpears"] = 0;
        }

        return driver.blackboard.targetEnemy != null && driver.blackboard.genericInts["bossStage"] == bossStage && driver.blackboard.genericInts["spawnedSpears"] > 0;
    }

    public override bool ShouldExit(AIDriver driver){
        return driver.blackboard.genericInts["spearAttackEndStage"] == 2 && driver.blackboard.genericTime <= 0f;
    }

}
