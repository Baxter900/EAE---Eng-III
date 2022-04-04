using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrinkOverTime : MonoBehaviour{
    private bool hasStarted = false;
    private float timePassed = 0f;

    private float duration;
    private AnimationCurve curve;

    private Vector3 initialSize;
    private Vector3 finalSize;


    public void Initialize(float duration, float finalSize, AnimationCurve curve = null){
        hasStarted = true;
        this.duration = duration;
        this.initialSize = transform.localScale;
        this.finalSize = transform.localScale * finalSize;
        if(curve != null){
            this.curve = curve;
        }else{
            curve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
        }
    }

    void Update(){
        if(!hasStarted){
            return;
        }

        timePassed += Time.deltaTime;
        transform.localScale = Vector3.Lerp(initialSize, finalSize, curve.Evaluate(timePassed / duration));
    }
}
