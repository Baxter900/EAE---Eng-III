using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[ExecuteInEditMode]
public class BorderVisualsController : MonoBehaviour{
    [SerializeField]
    private TileBase borderTile;

    void Start(){
        // int count = GridManager.Instance.dimensions.x * 2 + GridManager.Instance.dimensions.y * 2 - 4;

        int xDim = GridManager.Instance.dimensions.x + 1;
        int yDim = GridManager.Instance.dimensions.y + 1;
        for(int x = -xDim; x <= xDim; x++){
            for(int y = -yDim; y <= yDim; y++){
                if(x == -xDim || x == xDim || y == -yDim || y == yDim){
                    GridManager.Instance.tilemap.SetTile(new Vector3Int(x, y, 0), borderTile);
                }
            }
        }
    }

    #if UNITY_EDITOR

        void Update(){
            Start();
        }

    #endif // UNITY_EDITOR
}
