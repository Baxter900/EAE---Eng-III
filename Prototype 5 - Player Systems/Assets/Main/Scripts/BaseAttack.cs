using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Capsule{
    public Vector3 eulerAngles;
    public float length;
    public float radius;
}

public abstract class BaseAttack : ScriptableObject{
    public Sprite attackIcon;
    public abstract void OnSelected(CharacterControllerDriver driver);
    public abstract void OnDeselected(CharacterControllerDriver driver);
    public abstract void OnFixedUpdate(CharacterControllerDriver driver);
    public abstract void OnButtonPressStart(CharacterControllerDriver driver);
    public abstract void OnButtonPressEnd(CharacterControllerDriver driver);
}
