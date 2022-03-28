using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class ExitManager : MonoBehaviour{

    public void OnExit(InputAction.CallbackContext context){
        if(context.performed){
            Application.Quit();
        }
    }

    public void OnRestart(InputAction.CallbackContext context){
        if(context.performed){
            SceneManager.LoadScene(0);
        }
    }
}
