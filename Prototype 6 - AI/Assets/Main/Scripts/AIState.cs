using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AIState : ScriptableObject{
    public abstract void OnEnter(AIDriver driver);
    public abstract void OnUpdate(AIDriver driver);
    public abstract void OnExit(AIDriver driver);

    public abstract bool ShouldEnter(AIDriver driver);
    public abstract bool ShouldExit(AIDriver driver);
}
