using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlaceableType", menuName = "Placeable/PlaceableType", order = 1)]
public class PlaceableType : ScriptableObject{
    public string typeName;
    public Material[] materials;
}
