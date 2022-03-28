using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraFreeFlight : MonoBehaviour{

    public float speed = 10f;
    public float rotSpeed = 1f;

    private Vector2 angles;
    private Vector2 inputDir;

    void Update(){
        transform.rotation = Quaternion.Euler(-angles.y, angles.x, 0f);
        transform.position += (transform.forward * inputDir.y + transform.right * inputDir.x) * Time.deltaTime * speed;
    }

    public void OnLook(InputAction.CallbackContext context){
        Vector2 input = context.ReadValue<Vector2>();
        angles += input * rotSpeed;
    }

    public void OnMove(InputAction.CallbackContext context){
        Vector2 input = context.ReadValue<Vector2>();
        inputDir = input;
    }
}
