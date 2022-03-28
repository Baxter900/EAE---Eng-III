using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterControllerState : ScriptableObject{
    public abstract void OnEnter(CharacterControllerDriver driver);
    public abstract void OnFixedUpdate(CharacterControllerDriver driver);
    public abstract void OnExit(CharacterControllerDriver driver);

    public abstract bool ShouldEnter(CharacterControllerDriver driver);
    public abstract bool ShouldExit(CharacterControllerDriver driver);
}
