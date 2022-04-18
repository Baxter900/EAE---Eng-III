using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class DamageAura : MonoBehaviour{
    public float auraRange = 1f;
    public LayerMask hittableLayers;
    public float tickInterval = 0.1f;

    public float damageAmountPerTick = 1f;
    public string[] damageTags;

    private float t = 0f;

    public int maxHits = 1;

    private int numHits = 0;

    void Update(){
        t += Time.deltaTime;
        if(t >= tickInterval){
            t = 0f;
            DamageAuraTick();
        }
    }

    private void DamageAuraTick(){
        Collider[] hits = Physics.OverlapSphere(transform.position, auraRange, hittableLayers);
        foreach(Collider hit in hits){
            Health health = hit.GetComponentInParent<Health>();
            if(health){
                if(numHits < maxHits){
                    numHits++;
                    health.Hit(damageAmountPerTick, damageTags);
                }
            }
        }
    }
}
