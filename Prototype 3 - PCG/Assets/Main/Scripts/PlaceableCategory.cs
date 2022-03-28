using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlaceableCategory", menuName = "Placeable/PlaceableCategory", order = 1)]
public class PlaceableCategory : ScriptableObject{
    public string categoryName;
    public Placeable prefab;
    public PlaceableType[] types;

    public GameObject CreateAt(int typeIndex, Vector3 position, Quaternion rotation){
        if(typeIndex == -1){
            typeIndex = Random.Range(0, types.Length);
        }
        Placeable obj = Instantiate<Placeable>(prefab, position, rotation);
        PlaceableType type = types[Random.Range(0, types.Length)];
        for(int i = 0; i < obj.renderers.Length; i++){
            obj.renderers[i].material = type.materials[i % type.materials.Length];
        }

        return obj.gameObject;
    }
}
