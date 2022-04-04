using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EggExplodeState", menuName = "AI/Egg/Explode", order = 1)]
public class EggExplodeState : AIState{
    public float boomRange = 5f;
    public float countdownTimer = 0.1f;
    public LayerMask hittableLayers;

    public float damage = 10f;
    public string[] damageTags;

    public GameObject explodeParticlePrefab;

    public override void OnEnter(AIDriver driver){
        driver.blackboard.genericTime = countdownTimer;
        driver.blackboard.genericBools["hasExploded"] = false;
        driver.characterDriver.OnMove(Vector2.zero);

    }

    public override void OnUpdate(AIDriver driver){

        driver.blackboard.genericTime -= Time.deltaTime;
        if(driver.blackboard.genericTime <= 0f && !driver.blackboard.genericBools["hasExploded"]){
            driver.blackboard.genericBools["hasExploded"] = true;
            Health health = driver.GetComponentInParent<Health>();
            if(!health || health.IsAlive){
                Boom(driver);
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
        return shouldExplode;
    }

    public override bool ShouldExit(AIDriver driver){
        bool isSetup = driver.blackboard.genericBools.TryGetValue("shouldExplode", out bool shouldExplode);
        if(!isSetup){
            shouldExplode = false;
        }
        return !shouldExplode;
    }

    private void Boom(AIDriver driver){
        Instantiate(explodeParticlePrefab, driver.rb.position, Quaternion.identity, null);

        Collider[] hits = Physics.OverlapSphere(driver.rb.position, boomRange, hittableLayers);
        foreach(Collider hit in hits){
            Health health = hit.GetComponentInParent<Health>();
            if(health){
                health.Hit(damage, damageTags);
            }
        }
    }
}
