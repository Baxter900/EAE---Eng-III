using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class RespawnPointManager : MonoBehaviour{
    public static RespawnPointManager Instance = null;

    public RespawnPoint currentRespawnPoint = null;

    void Awake(){
        if(Instance == null){
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }else{
            Destroy(this.gameObject);
        }
    }

    public static void Respawn(Transform obj){
        if(!RespawnPointManager.Instance || !RespawnPointManager.Instance.currentRespawnPoint){
            return;
        }
        Instantiate(RespawnPointManager.Instance.currentRespawnPoint.enableOnSpawn, RespawnPointManager.Instance.currentRespawnPoint.enableOnSpawn.transform.parent).SetActive(true);
        obj.position = RespawnPointManager.Instance.currentRespawnPoint.transform.position;
        obj.rotation = RespawnPointManager.Instance.currentRespawnPoint.transform.rotation;
    }
}
