using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour{
    private BlockType type;
    [HideInInspector]
    public TerrainBuilder terrain;
    private Vector3Int blockLocation;

    private Renderer[] renderers;

    public BlockType Type{
        get{
            return type;
        }
    }

    void Awake(){
        renderers = GetComponentsInChildren<Renderer>();
    }

    public void SetTerrain(TerrainBuilder owner, Vector3Int newLocation){
        terrain = owner;
        blockLocation = newLocation;
    }

    public void MakeType(BlockType newType){
        type = newType;
        foreach(Renderer render in renderers){
            render.material = newType.material;
        }
    }
}
