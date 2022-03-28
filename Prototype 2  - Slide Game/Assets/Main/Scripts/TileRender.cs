using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[ExecuteInEditMode]
[RequireComponent(typeof(GridTransform))]
public class TileRender : MonoBehaviour{
    public TileBase tile;
    public Color color = Color.white;

    private GridTransform gridTransform;

    void Awake(){
        gridTransform = GetComponent<GridTransform>();
    }

    void Start(){
        RenderVisuals();
    }

    void Update(){

    }

    public void ClearVisuals(){
        GridManager.Instance.RenderBox(gridTransform.position, null, gridTransform.dimensions.x, gridTransform.dimensions.y);
    }

    public void RenderVisuals(){
        GridManager.Instance.RenderBox(gridTransform.position, tile, gridTransform.dimensions.x, gridTransform.dimensions.y);
        GridManager.Instance.ColorBox(gridTransform.position, color, gridTransform.dimensions.x, gridTransform.dimensions.y);
    }
}
