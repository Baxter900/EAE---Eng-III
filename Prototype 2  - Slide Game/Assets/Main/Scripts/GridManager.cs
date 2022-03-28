using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[ExecuteInEditMode]
public class GridManager : MonoBehaviour{
    private static GridManager _instance = null;
    public static GridManager Instance{
        get{
            return _instance;
        }
        set{
            Debug.Assert(_instance == null, "Tried to create a second GridManager. There can only be one GridManager at a time.");
            _instance = value;
        }
    }

    void Awake(){
        Instance = this;
    }

    [SerializeField]
    public Grid grid;

    [SerializeField]
    public Tilemap tilemap;

    [SerializeField]
    public Vector2Int dimensions;

    public void RenderBox(Vector3Int position, TileBase tile, int dimX, int dimY){
        tilemap.BoxFill(position, tile, position.x, position.y, position.x + dimX, position.y + dimY);
    }

    public void ColorBox(Vector3Int position, Color color, int dimX, int dimY){
        for(int x = position.x; x <= position.x + dimX; x++){
            for(int y = position.y; y <= position.y + dimY; y++){
                Vector3Int pos = new Vector3Int(x, y, 0);

                tilemap.SetTileFlags(pos, TileFlags.None);
                tilemap.SetColor(pos, color);
            }
        }
    }

}
