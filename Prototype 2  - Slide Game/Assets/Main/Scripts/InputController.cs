using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour{

    private GridTransform carried = null;
    private Vector2Int lastMousePos;

    void Start(){
        lastMousePos = GetMouseCellPosition2D();
    }

    public void OnQuit(InputAction.CallbackContext context){
        if(context.performed){
            Application.Quit();
        }
    }

    public void OnUndo(InputAction.CallbackContext context){
        if(context.performed){
            CommandSystem.Undo();
        }
    }

    public void OnRedo(InputAction.CallbackContext context){
        if(context.performed){
            CommandSystem.Redo();
        }
    }

    public void OnCarry(InputAction.CallbackContext context){
        if(context.performed){
            if(!WinDetector.hasWon && context.ReadValueAsButton()){
                carried = CollisionSystem.GetObjectAt(GetMouseCellPosition3D());
            }else{
                carried = null;
                CommandSystem.StartNewSet();
            }
        }
    }

    void Update(){
        if(carried && lastMousePos != GetMouseCellPosition2D()){
            Command newCommand = new Command(carried, GetMouseCellPosition2D() - lastMousePos);
            CommandSystem.PerformCommand(newCommand);
            SFXManager.MoveSFX();
        }

        lastMousePos = GetMouseCellPosition2D();
    }


    private Vector3Int GetMouseCellPosition3D(){
        Vector3Int position = GridManager.Instance.grid.WorldToCell(Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()));
        return position;
    }

    private Vector2Int GetMouseCellPosition2D(){
        Vector3Int pos3D = GetMouseCellPosition3D();
        Vector2Int pos2D = new Vector2Int(pos3D.x, pos3D.y);
        return pos2D;
    }
}
