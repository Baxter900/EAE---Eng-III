using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class MoveTowardTransform : MonoBehaviour{
    public Transform target;
    public Vector3 targetPosition;
    public bool useTargetTransform = false;
    public bool isActive = false;
    public float speed;
    public float closeEnough = 0.5f;

    void Update(){
        if(!isActive){
            return;
        }

        Vector3 currentTargetPosition = targetPosition;
        if(useTargetTransform && target){
            currentTargetPosition = transform.position;
        }
        Vector3 direction = (currentTargetPosition - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;

        if(Vector3.Distance(currentTargetPosition, transform.position) <= closeEnough){
            isActive = false;
        }
    }
}
