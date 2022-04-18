using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class RespawnOnStart : MonoBehaviour{

    void Start(){
        RespawnPointManager.Respawn(this.transform);
    }
}
