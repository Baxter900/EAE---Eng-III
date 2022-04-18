using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "BossBackToPreviousState", menuName = "AI/Boss/BackToPreviousStage", order = 1)]
public class BossBackToPreviousStageState : AIState{
    public int bossStage;
    public int newBossStage;

    public override void OnEnter(AIDriver driver){
        driver.blackboard.genericInts["bossStage"] = newBossStage;
    }

    public override void OnUpdate(AIDriver driver){


    }

    public override void OnExit(AIDriver driver){

    }


    public override bool ShouldEnter(AIDriver driver){
        if(!driver.blackboard.genericInts.ContainsKey("bossStage")){
            driver.blackboard.genericInts["bossStage"] = 0;
        }

        return driver.blackboard.targetEnemy != null
            && driver.blackboard.genericInts["bossStage"] == bossStage;
    }

    public override bool ShouldExit(AIDriver driver){
        return true;
    }

}
