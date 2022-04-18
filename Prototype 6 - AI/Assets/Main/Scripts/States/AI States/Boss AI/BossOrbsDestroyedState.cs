using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "BossOrbsDestroyedState", menuName = "AI/Boss/OrbsDestroyed", order = 1)]
public class BossOrbsDestroyedState : AIState{
    public int numOrbsDestroyedToTrigger = 2;
    public int bossStage;

    public override void OnEnter(AIDriver driver){
        driver.blackboard.genericInts["bossStage"]++;
        driver.blackboard.genericInts["bossOrbsDestroyed"] = 0;
    }

    public override void OnUpdate(AIDriver driver){


    }

    public override void OnExit(AIDriver driver){

    }


    public override bool ShouldEnter(AIDriver driver){
        if(!driver.blackboard.genericInts.ContainsKey("bossStage")){
            driver.blackboard.genericInts["bossStage"] = 0;
        }

        if(!driver.blackboard.genericInts.ContainsKey("bossOrbsDestroyed")){
            driver.blackboard.genericInts["bossOrbsDestroyed"] = 0;
        }


        return driver.blackboard.targetEnemy != null
            && driver.blackboard.genericInts["bossStage"] == bossStage
            && driver.blackboard.genericInts["bossOrbsDestroyed"] >= numOrbsDestroyedToTrigger;
    }

    public override bool ShouldExit(AIDriver driver){
        return true;
    }

}
