using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WinDetector : MonoBehaviour{
    public GridTransform trackedObject;
    public Vector2Int winLocation;

    public GameObject showOnWin;

    public static bool hasWon = false;

    void Start(){
        SetWin(false);
    }

    void Update(){
        if(!hasWon && trackedObject.position2D == winLocation){
            SetWin(true);
        }
    }

    public void SetWin(bool value){
        showOnWin.SetActive(value);
        hasWon = value;
        if(value){
            SFXManager.WinSFX();
        }
    }

    public void OnUndo(InputAction.CallbackContext context){
        if(context.performed){
            if(hasWon){
                SetWin(false);
            }
        }
    }
}
