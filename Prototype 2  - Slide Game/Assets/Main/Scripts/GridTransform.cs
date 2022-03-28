using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class GridTransform : MonoBehaviour{
    [SerializeField]
    private Vector3Int _position;
    public Vector3Int position{
        get{
            return _position;
        }
        set{
            if(render){
                render.ClearVisuals();
            }
            _position = value;
            EnforceBounds();
            transform.position = GridManager.Instance.grid.CellToWorld(_position);
            if(render){
                render.RenderVisuals();
            }
        }
    }

    public Vector2Int position2D{
        get{
            return new Vector2Int(_position.x, _position.y);
        }
        set{
            position = new Vector3Int(value.x, value.y, 0);
        }
    }

    [SerializeField]
    private Vector2Int _dimensions;

    public Vector2Int dimensions{
        get{
            return _dimensions - Vector2Int.one;
        }
        set{
            _dimensions = value;
        }
    }

    private Vector3 lastWorldPosition;
    private TileRender render;

    void Awake(){
        lastWorldPosition = transform.position;
        render = GetComponent<TileRender>();
    }

    void Start(){
        CollisionSystem.Register(this);
    }

    void OnDestroy(){
        CollisionSystem.Unregister(this);
    }

    void Update(){
        if(lastWorldPosition != transform.position){
            position = GridManager.Instance.grid.WorldToCell(transform.position);
            lastWorldPosition = transform.position;
        }

    }

    private void EnforceBounds(){
        if(_position.x > GridManager.Instance.dimensions.x - dimensions.x){
            _position.x = GridManager.Instance.dimensions.x - dimensions.x;
        }else if(_position.x < -GridManager.Instance.dimensions.x){
            _position.x = -GridManager.Instance.dimensions.x;
        }

        if(_position.y > GridManager.Instance.dimensions.y - dimensions.y){
            _position.y = GridManager.Instance.dimensions.y - dimensions.y;
        }else if(_position.y < -GridManager.Instance.dimensions.y){
            _position.y = -GridManager.Instance.dimensions.y;
        }
    }

    public IEnumerator<Vector3Int> GetEnumerator(){
        for(int x = position.x; x <= position.x + dimensions.x; x++){
            for(int y = position.y; y <= position.y + dimensions.y; y++){
                yield return new Vector3Int(x, y, 0);
            }
        }
    }

    public IEnumerator<Vector3Int> IterateTilePositions(Vector3Int position, Vector2Int dims){
        for(int x = position.x; x <= position.x + dims.x; x++){
            for(int y = position.y; y <= position.y + dims.y; y++){
                yield return new Vector3Int(x, y, 0);
            }
        }
    }

    public Vector3IntEnumerable GetTilePositionIterator(Vector3Int position, Vector2Int dims){
        return new Vector3IntEnumerable(IterateTilePositions(position, dims));
    }

    public class Vector3IntEnumerable{
        public IEnumerator<Vector3Int> enumerator;

        public Vector3IntEnumerable(IEnumerator<Vector3Int> enumerator){
            this.enumerator = enumerator;
        }

        public IEnumerator<Vector3Int> GetEnumerator(){
            return enumerator;
        }
    }
}
