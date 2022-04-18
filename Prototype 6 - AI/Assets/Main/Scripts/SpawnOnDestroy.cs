using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class SpawnOnDestroy : MonoBehaviour{
    public GameObject spawnOnDestroy;

    void OnDestroy(){
        Instantiate(spawnOnDestroy, transform.position, Quaternion.identity);
    }


}
