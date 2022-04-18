using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class BossEnvironmentController : MonoBehaviour{
    public GameObject[] objectsToEnableOnEnterStage1;
    public GameObject[] objectsToCopyAndSpawnOnUnstun1;
    public GameObject[] objectsToCopyAndSpawnOnUnstun2;

    public BossIdleState idleState;
    public BossStunnedState stunnedState1;
    public BossStunnedState stunnedState2;

    void Start(){
        idleState.playerDetected.AddListener(OnEnterStage1);
        stunnedState1.unstunnedEvent.AddListener(OnUnstun1);
        stunnedState2.unstunnedEvent.AddListener(OnUnstun2);

        foreach(GameObject obj in objectsToEnableOnEnterStage1){
            obj.SetActive(false);
        }

        foreach(GameObject obj in objectsToCopyAndSpawnOnUnstun1){
            obj.SetActive(false);
        }

        foreach(GameObject obj in objectsToCopyAndSpawnOnUnstun2){
            obj.SetActive(false);
        }
    }

    private void OnEnterStage1(){
        foreach(GameObject obj in objectsToEnableOnEnterStage1){
            obj.SetActive(true);
        }
    }

    private void OnUnstun1(){
        foreach(GameObject obj in objectsToCopyAndSpawnOnUnstun1){
            GameObject newObj = Instantiate(obj);
            newObj.SetActive(true);
        }
    }

    private void OnUnstun2(){
        foreach(GameObject obj in objectsToCopyAndSpawnOnUnstun2){
            GameObject newObj = Instantiate(obj);
            newObj.SetActive(true);
        }
    }
}
