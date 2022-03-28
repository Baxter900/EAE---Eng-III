using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Exiter : MonoBehaviour{

    public void OnQuit(InputAction.CallbackContext context){
        if(context.performed){
            Application.Quit();
        }
    }
}
