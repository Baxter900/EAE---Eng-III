using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CommandSystem{
    public static Stack<CommandSet> commandStack = new Stack<CommandSet>();

    public static Stack<CommandSet> redoStack = new Stack<CommandSet>();

    private static CommandSet currentSet = new CommandSet();

    public static void StartNewSet(){
        if(currentSet.Count > 0){
            commandStack.Push(currentSet);
            currentSet = new CommandSet();
        }
    }

    public static void PerformCommand(Command command){
        if(command.Perform()){
            currentSet.AddCommandToSet(command);
            redoStack.Clear();
        }
    }

    public static void Undo(){
        if(currentSet.Count > 0){
            StartNewSet();
        }

        if(commandStack.Count > 0){
            CommandSet commands = commandStack.Pop();
            commands.UndoSet();
            redoStack.Push(commands);
            SFXManager.UndoSFX();
        }else{
            SFXManager.FailedSFX();
        }
    }

    public static void Redo(){
        if(redoStack.Count > 0){
            StartNewSet();

            CommandSet commands = redoStack.Pop();
            commands.PerformSet();

            commandStack.Push(commands);
            SFXManager.RedoSFX();
        }else{
            SFXManager.FailedSFX();
        }
    }


}

public class CommandSet{
    private List<Command> commandList = new List<Command>();

    public int Count{
        get{
            return commandList.Count;
        }
    }

    public void AddCommandToSet(Command command){
        commandList.Add(command);
    }

    public void PerformSet(){
        for(int i = 0; i < commandList.Count; i++){
            commandList[i].Perform();
        }
    }

    public void UndoSet(){
        for(int i = commandList.Count - 1; i >= 0; i--){
            commandList[i].SafeUndo();
        }
    }
}

public class Command{
    public GridTransform gridTransform;
    public Vector2Int movement;

    private Vector2Int actualMovement;
    private bool hasBeenPerformed = false;

    public Command(GridTransform gridTransform, Vector2Int movement){
        this.gridTransform = gridTransform;
        this.movement = movement;
    }

    public bool Perform(){
        hasBeenPerformed = true;
        if(CheckNoCollision()){
            Vector2Int oldPos = gridTransform.position2D;
            gridTransform.position2D += movement;
            actualMovement = gridTransform.position2D - oldPos;
            Debug.LogFormat("Input Movement: {0}, Actual Movement: {1}", movement, actualMovement);
        }else{
            actualMovement = Vector2Int.zero;
        }

        return actualMovement != Vector2Int.zero;
    }

    public bool CheckNoCollision(){
        return !CollisionSystem.DoesCollide(this.gridTransform, this.gridTransform.position + new Vector3Int(movement.x, movement.y, 0));
    }

    public void SafeUndo(){
        Debug.Assert(hasBeenPerformed);
        Undo();
    }

    private void Undo(){
        gridTransform.position2D -= actualMovement;
    }

}
