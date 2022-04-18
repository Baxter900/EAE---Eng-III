using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class BossMechanicOrb : MonoBehaviour{
    public AIDriver driver;

    void OnDestroy(){
        if(!driver.blackboard.genericInts.ContainsKey("bossOrbsDestroyed")){
            driver.blackboard.genericInts["bossOrbsDestroyed"] = 0;
        }

        driver.blackboard.genericInts["bossOrbsDestroyed"]++;

    }
}
