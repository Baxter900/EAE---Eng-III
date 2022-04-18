using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "BossSpearAttackStateStart", menuName = "AI/Boss/SpearAttackStart", order = 1)]
public class BossSpearAttackStateStart : AIState{
    public GameObject spearPrefab;
    public int numberOfSpearsToSpawn = 9;
    public float spearSpawnInterval = 0.15f;
    public Vector3 spearSpawnOriginOffset = Vector3.zero;
    public Vector3 alternatingSpawnOffsetPerSpear = Vector3.zero;
    public int bossStage = 2;


    public override void OnEnter(AIDriver driver){
        driver.blackboard.genericInts["remainingSpearsToSpawn"] = numberOfSpearsToSpawn;
        driver.blackboard.genericInts["spawnedSpears"] = 0;
        driver.blackboard.genericTime = spearSpawnInterval;

        foreach(GameObject obj in driver.blackboard.tempCreatedObjects){
            Destroy(obj);
        }
    }

    public override void OnUpdate(AIDriver driver){
        int remainingSpears = driver.blackboard.genericInts["remainingSpearsToSpawn"];

        driver.blackboard.genericTime -= Time.deltaTime;
        if(driver.blackboard.genericTime <= 0f && remainingSpears > 0){
            driver.blackboard.genericTime = spearSpawnInterval;

            GameObject newSpear = Instantiate(spearPrefab);
            LookAtTransform lookAt = newSpear.GetComponent<LookAtTransform>();
            lookAt.target = driver.blackboard.targetEnemy;
            lookAt.useTargetTransform = true;

            Vector3 worldAlternatingSpawnOffsetPerSpear = driver.rb.transform.TransformVector(alternatingSpawnOffsetPerSpear).normalized * alternatingSpawnOffsetPerSpear.magnitude;
            Vector3 spearSpecificOffset = (remainingSpears % 2 == 0 ? 1f : -1f) * (worldAlternatingSpawnOffsetPerSpear * (remainingSpears - 1));
            newSpear.transform.position = driver.rb.transform.position + spearSpawnOriginOffset + spearSpecificOffset;

            driver.blackboard.tempCreatedObjects.Add(newSpear);
            driver.blackboard.genericInts["remainingSpearsToSpawn"]--;
            driver.blackboard.genericInts["spawnedSpears"]++;
        }

    }

    public override void OnExit(AIDriver driver){

    }



    public override bool ShouldEnter(AIDriver driver){
        if(!driver.blackboard.genericInts.ContainsKey("bossStage")){
            driver.blackboard.genericInts["bossStage"] = 0;
        }

        if(!driver.blackboard.genericInts.ContainsKey("spawnedSpears")){
            driver.blackboard.genericInts["spawnedSpears"] = 0;
        }

        return driver.blackboard.targetEnemy != null && driver.blackboard.genericInts["bossStage"] == bossStage && driver.blackboard.genericInts["spawnedSpears"] == 0;
    }

    public override bool ShouldExit(AIDriver driver){
        return driver.blackboard.genericInts["remainingSpearsToSpawn"] == 0;
    }

}
