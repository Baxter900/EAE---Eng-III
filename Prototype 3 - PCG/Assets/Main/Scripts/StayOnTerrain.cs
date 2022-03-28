using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StayOnTerrain : MonoBehaviour{
    public Transform visuals;

    void Update(){
        UpdateVisuals();
    }

    private void UpdateVisuals(){
        Vector3 position = visuals.position;
        position.y = TerrainBuilder.GetTerrainBuilder(transform.position).GetHeightAtWorldPosition2D(GetPosition2D()) - 0.5f;
        visuals.position = position;
    }

    private Vector2 GetPosition2D(){
        return new Vector2(transform.position.x, transform.position.z);
    }
}
