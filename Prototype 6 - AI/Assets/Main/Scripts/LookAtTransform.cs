using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class LookAtTransform : MonoBehaviour{
    public Transform target;
    public Vector3 targetPosition;
    public bool useTargetTransform = false;
    public bool onlyRotateY = false;

    void Update(){
        Vector3 position = targetPosition;
        if(useTargetTransform){
            if(!target){
                return;
            }
            position = target.position;
        }
        if(onlyRotateY){
            position.y = transform.position.y;
        }
        transform.LookAt(position);
    }
}
