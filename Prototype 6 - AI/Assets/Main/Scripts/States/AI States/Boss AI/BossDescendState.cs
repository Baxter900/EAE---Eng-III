using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "BossDescendState", menuName = "AI/Boss/Descend", order = 1)]
public class BossDescendState : AIState{

    public float desiredHeight = 12f;
    public float descendSpeed = 6f;
    public float closeEnough = 1f;

    public int bossStage = 1;

    public override void OnEnter(AIDriver driver){
        driver.blackboard.genericBools["isNearHeight"] = false;
    }

    public override void OnUpdate(AIDriver driver){
        float dir = Mathf.Sign(desiredHeight - driver.rb.transform.position.y);
        driver.rb.transform.position += dir * Vector3.up * descendSpeed * Time.deltaTime;
        if(Mathf.Abs(driver.rb.transform.position.y - desiredHeight) <= closeEnough){
            driver.blackboard.genericBools["isNearHeight"] = true;
            driver.blackboard.genericInts["bossStage"]++;
        }

    }

    public override void OnExit(AIDriver driver){

    }


    public override bool ShouldEnter(AIDriver driver){
        if(!driver.blackboard.genericInts.ContainsKey("bossStage")){
            driver.blackboard.genericInts["bossStage"] = 0;
        }

        return driver.blackboard.targetEnemy != null && driver.blackboard.genericInts["bossStage"] == bossStage;
    }

    public override bool ShouldExit(AIDriver driver){
        return driver.blackboard.genericBools["isNearHeight"];
    }

}
