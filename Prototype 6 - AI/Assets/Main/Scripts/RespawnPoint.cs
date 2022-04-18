using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class RespawnPoint : MonoBehaviour{
    public GameObject enableOnSpawn;

    void Start(){
        enableOnSpawn.SetActive(false);
    }

    public void OnTriggerEnter(Collider other){
        Debug.Log("Trigger enter");
        if(other.tag == "Player"){
            Debug.Log("tag is player");
            RespawnPointManager.Instance.currentRespawnPoint = this;
        }
    }
}
