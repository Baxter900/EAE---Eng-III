using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GeneralKeybindHandler : MonoBehaviour{

    public void OnPressQuit(InputAction.CallbackContext context){
        if(context.performed){
            Application.Quit();
        }
    }
}
