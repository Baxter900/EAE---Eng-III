using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraRotator : MonoBehaviour{
    public float sensitivity = 0.01f;
    public Vector2 verticalLookLimits;
    private Vector2 lookAngle;

    void Start(){
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void OnLook(InputAction.CallbackContext context){
        lookAngle.x += context.ReadValue<Vector2>().x * sensitivity;
        lookAngle.y += -context.ReadValue<Vector2>().y * sensitivity;


        lookAngle.x %= 360f;
        if(lookAngle.x > 360f){
            lookAngle.x -= 360f;
        }else if(lookAngle.x < 0f){
            lookAngle.x += 360f;
        }

        lookAngle.y = Mathf.Clamp(lookAngle.y, verticalLookLimits[0], verticalLookLimits[1]);

        transform.rotation = Quaternion.Euler(lookAngle.y, lookAngle.x, 0f);
    }
}
