using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "BossSpawnEnemyState", menuName = "AI/Boss/SpawnEnemy", order = 1)]
public class BossSpawnEnemyState : AIState{
    public GameObject enemyPrefab;
    public int numberofEnemiesToSpawn = 2;
    public Vector3 enemySpawnOffsetAlternating;
    public float duration = 3f;
    public int bossStage = 2;
    public bool doesNeedPriorActivation = false;


    public override void OnEnter(AIDriver driver){
        driver.blackboard.genericBools["canSpawnEnemiesPriorActivation"] = true;
        driver.blackboard.genericBools["isEnemySpawnBlockedUntilStun"] = true;
        driver.blackboard.genericBools["hasSpawnedYet"] = false;
        driver.blackboard.genericTime = duration;

    }

    public override void OnUpdate(AIDriver driver){
        if(driver.blackboard.genericBools["hasSpawnedYet"]){
            return;
        }

        driver.blackboard.genericTime -= Time.deltaTime;

        if(driver.blackboard.genericTime <= 0f){
            driver.blackboard.genericBools["hasSpawnedYet"] = true;
            for(int i = 0; i < numberofEnemiesToSpawn; i++){
                GameObject enemy = Instantiate(enemyPrefab);

                Vector3 worldAlternatingSpawnOffsetPerEnemy = driver.rb.transform.TransformVector(enemySpawnOffsetAlternating).normalized * enemySpawnOffsetAlternating.magnitude;
                Vector3 enemySpecificOffset = (i % 2 == 0 ? 1f : -1f) * (worldAlternatingSpawnOffsetPerEnemy * i);
                enemy.transform.position = driver.rb.transform.position + enemySpecificOffset;

                AIDriver minionAI = enemy.GetComponentInChildren<AIDriver>();
                minionAI.blackboard.targetEnemy = driver.blackboard.targetEnemy;
            }
        }
    }

    public override void OnExit(AIDriver driver){

    }



    public override bool ShouldEnter(AIDriver driver){
        if(!driver.blackboard.genericInts.ContainsKey("bossStage")){
            driver.blackboard.genericInts["bossStage"] = 0;
        }

        if(!driver.blackboard.genericBools.ContainsKey("canSpawnEnemiesPriorActivation")){
            driver.blackboard.genericBools["canSpawnEnemiesPriorActivation"] = false;
        }

        if(!driver.blackboard.genericBools.ContainsKey("isEnemySpawnBlockedUntilStun")){
            driver.blackboard.genericBools["isEnemySpawnBlockedUntilStun"] = true;
        }

        return driver.blackboard.targetEnemy != null
            && driver.blackboard.genericInts["bossStage"] == bossStage
            && (!doesNeedPriorActivation || driver.blackboard.genericBools["canSpawnEnemiesPriorActivation"])
            && Random.value < 0.25f
            ;   //&& !driver.blackboard.genericBools["isEnemySpawnBlockedUntilStun"];
    }

    public override bool ShouldExit(AIDriver driver){
        return driver.blackboard.genericTime <= 0f;
    }

}
