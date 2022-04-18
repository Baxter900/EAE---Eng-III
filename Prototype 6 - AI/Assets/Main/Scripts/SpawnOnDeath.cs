using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class SpawnOnDeath : MonoBehaviour{
    public GameObject spawnOnDestroy;
    public Health health;

    void Start(){
        health.deathEvent.AddListener(OnDeath);
    }

    private void OnDeath(){
        Instantiate(spawnOnDestroy, transform.position, Quaternion.identity);
    }


}
