using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "BossStunnedState", menuName = "AI/Boss/BossStunned", order = 1)]
public class BossStunnedState : AIState{
    public float stunnedDuration = 10f;
    public int bossStage;

    public float desiredHeight = 1f;
    public float descendSpeed = 6f;
    public float closeEnough = 1f;

    public string[] damageTags;

    public Material stunnedMaterial;
    public Material normalMaterial;

    public UnityEvent stunnedEvent = new UnityEvent();
    public UnityEvent unstunnedEvent = new UnityEvent();

    public override void OnEnter(AIDriver driver){
        driver.blackboard.genericBools["isEnemySpawnBlockedUntilStun"] = false;

        driver.blackboard.genericBools["isNearHeight"] = false;
        driver.blackboard.genericTime = stunnedDuration;

        foreach(Renderer render in driver.rb.GetComponentsInChildren<Renderer>()){
            Material[] mats = render.materials;
            for(int i = 0; i < render.materials.Length; i++){
                mats[i] = stunnedMaterial;
            }
            render.materials = mats;
        }

        driver.rb.GetComponent<Health>().SetTags(damageTags);

        driver.blackboard.genericInts["bossStage"]++;

        stunnedEvent.Invoke();
    }

    public override void OnUpdate(AIDriver driver){
        if(!driver.blackboard.genericBools["isNearHeight"]){
            float dir = Mathf.Sign(desiredHeight - driver.rb.transform.position.y);
            driver.rb.transform.position += dir * Vector3.up * descendSpeed * Time.deltaTime;
            if(driver.rb.transform.position.y - desiredHeight <= closeEnough){
                driver.blackboard.genericBools["isNearHeight"] = true;
            }
        }else{
            driver.blackboard.genericTime -= Time.deltaTime;
        }

    }

    public override void OnExit(AIDriver driver){

        foreach(Renderer render in driver.rb.GetComponentsInChildren<Renderer>()){
            Material[] mats = render.materials;
            for(int i = 0; i < render.materials.Length; i++){
                mats[i] = normalMaterial;
            }
            render.materials = mats;
        }

        driver.rb.GetComponent<Health>().SetTags(new string[0]);

        driver.blackboard.genericBools["isNearHeight"] = false;
        unstunnedEvent.Invoke();
    }


    public override bool ShouldEnter(AIDriver driver){
        if(!driver.blackboard.genericInts.ContainsKey("bossStage")){
            driver.blackboard.genericInts["bossStage"] = 0;
        }


        return driver.blackboard.targetEnemy != null
            && driver.blackboard.genericInts["bossStage"] == bossStage;
    }

    public override bool ShouldExit(AIDriver driver){
        return driver.blackboard.genericTime <= 0f;
    }

}
