using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainBuilder : MonoBehaviour{
    private static TerrainBuilder _instance;

    public int seed = -1;
    public Vector3Int size;
    public float terrainNoiseScale = 10f;
    // public float snowNoiseScale = 10f;
    // public float snowThreshold = 0.8f;

    public int numObjects = 10;

    public Block blockPrefab;
    public BlockType[] terrainBlockTypes;
    // public BlockType snowBlockType;


    public BlockLibrary blockLibrary;

    public PlaceableCategory[] placeables;

    private Block[,,] blocks;
    private int[,] heights;

    private HashSet<Vector2Int> spacesWithObjects = new HashSet<Vector2Int>();
    private HashSet<GameObject> existingPlaceables = new HashSet<GameObject>();

    void Awake(){
        _instance = this;


    }

    public void MakeWorld(){
        // Set RNG
        // if(seed >= 0){
        //     Random.InitState(seed);
        // }
        CleanTerrain();

        MakeTerrain();
        MakeObjects();
        // MakeSnow();
    }

    private void MakeTerrain(){
        blocks = new Block[size.x, size.y, size.z];
        heights = new int[size.x, size.z];
        Vector2Int perlinOffset = new Vector2Int(Random.Range(-100000, 100000), Random.Range(-100000, 100000));
        for(int x = 0; x < size.x; x++){
            for(int z = 0; z < size.z; z++){
                float noise = Mathf.PerlinNoise((x / terrainNoiseScale + perlinOffset.x), (z / terrainNoiseScale + perlinOffset.y));
                // Debug.Log(noise);
                float heightF = Mathf.Lerp(0, size.y, noise);
                int height = Mathf.Clamp(Mathf.RoundToInt(heightF), 1, size.y);
                heights[x, z] = height;
                for(int y = 0; y < height; y++){
                    MakeBlock(x, y, z);
                }
            }
        }
    }

    private void MakeObjects(){
        for(int i = 0; i < numObjects; i++){
            PlaceableCategory obj = placeables[Random.Range(0, placeables.Length)];

            Vector2Int position = Vector2Int.zero;
            bool foundPosition = false;
            int tries = 0;
            while(!foundPosition){
                position = new Vector2Int(Random.Range(-size.x / 2, size.x / 2), Random.Range(-size.z / 2, size.z / 2));
                if(!spacesWithObjects.Contains(position)){
                    foundPosition = true;
                    spacesWithObjects.Add(position);
                }

                tries++;
                if(tries > 100){
                    return;
                }
            }

            Vector3 position3D = new Vector3(position.x, GetHeightAtWorldPosition2D(position) - 0.5f, position.y);
            Quaternion rotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
            existingPlaceables.Add(obj.CreateAt(-1, position3D, rotation));
        }
    }

    private void MakeBlock(int x, int y, int z){
        blocks[x, y, z] = Instantiate<Block>(blockPrefab, transform.position + new Vector3(x, y, z) - (new Vector3(size.x, size.y, size.z) / 2f), transform.rotation, transform);
        blocks[x, y, z].MakeType(terrainBlockTypes[y]);
        blocks[x, y, z].SetTerrain(this, new Vector3Int(x, y, z));
    }

    // private void MakeSnow(){
    //     Vector2Int perlinOffset = new Vector2Int(Random.Range(-100000, 100000), Random.Range(-100000, 100000));
    //     for(int x = 0; x < size.x; x++){
    //         for(int z = 0; z < size.z; z++){
    //             float noise = Mathf.PerlinNoise((x / snowNoiseScale + perlinOffset.x), (z / snowNoiseScale + perlinOffset.y));
    //             if(noise > snowThreshold){
    //                 for(int y = 0; y < heights[x, z]; y++){
    //                     Block block = blocks[x, y, z];
    //                     block.MakeType(snowBlockType);
    //                 }
    //             }
    //         }
    //     }
    // }

    public float GetHeightAtWorldPosition2D(Vector2 worldPosition){
        return GetHeightAtBlockPosition2D(WorldPosition2DToBlockPosition2D(worldPosition)) - GetBounds().extents.y;
    }

    public float GetHeightAtBlockPosition2D(Vector2Int blockPosition){
        return heights[blockPosition.x, blockPosition.y];
    }

    public Vector2Int WorldPosition2DToBlockPosition2D(Vector2 worldPosition2D){
        Vector3 worldPosition = new Vector3(worldPosition2D.x, 0f, worldPosition2D.y);
        Vector3 localPosition = transform.InverseTransformPoint(worldPosition);
        Vector2Int blockPosition = new Vector2Int();
        blockPosition.x = Mathf.RoundToInt(localPosition.x + GetBounds().extents.x);
        blockPosition.y = Mathf.RoundToInt(localPosition.z + GetBounds().extents.z);
        // Debug.Log(blockPosition);
        return blockPosition;
    }

    public bool RemoveBlockAtBlockLocation2D(Vector2Int location){
        int heightAtLocation = heights[location.x, location.y];
        if(heightAtLocation <= 1){
            return false;
        }

        Vector3Int topMostBlockLocation = new Vector3Int(location.x, heightAtLocation - 1, location.y);

        if(!RemoveBlockAtBlockLocation3D(topMostBlockLocation)){
            return false;
        }
        heights[location.x, location.y] -= 1;
        return true;
    }

    // Available for debug purposes
    public bool DebugRemoveBlockAtBlockLocation3D(Vector3Int location){
        return RemoveBlockAtBlockLocation3D(location);
    }

    // Using this without adjusting heights properly will probably break things
    private bool RemoveBlockAtBlockLocation3D(Vector3Int location){
        if(!IsBlockAt(location)){
            return false;
        }
        Block block = blocks[location.x, location.y, location.z];
        blocks[location.x, location.y, location.z] = null;
        GameObject.Destroy(block.gameObject);
        return true;
    }

    public bool AddBlockAtLocation2D(Vector2Int location){
        int heightAtLocation = heights[location.x, location.y];
        if(heightAtLocation >= 5){
            return false;
        }


        Vector3Int topMostBlockLocation = new Vector3Int(location.x, heightAtLocation, location.y);

        MakeBlock(topMostBlockLocation.x, topMostBlockLocation.y, topMostBlockLocation.z);
        heights[location.x, location.y] += 1;

        return true;
    }

    private void RecalculateHeights(){
        heights = new int[size.x, size.z];
        for(int x = 0; x < size.x; x++){
            for(int z = 0; z < size.z; z++){
                for(int y = size.y - 1; y >= 0; y--){
                    if(x == 17 && z == 12){
                    }
                    Block block = blocks[x, y, z];
                    if(block != null){
                        heights[x, z] = y + 1;
                        break;
                    }
                }
            }
        }
    }

    public bool IsBlockAt(Vector3Int blockLocation){
        Block block = blocks[blockLocation.x, blockLocation.y, blockLocation.z];
        return block != null;
    }

    public Bounds GetBounds(){
        return new Bounds(transform.position, size);
    }

    public static TerrainBuilder GetTerrainBuilder(Vector3 position){
        return _instance;
    }

    public void CleanTerrain(){
        // Set RNG
        if(seed >= 0){
            Random.InitState(seed);
        }

        if(blocks != null){
            foreach(Block block in blocks){
                if(block != null){
                    GameObject.Destroy(block.gameObject);
                }
            }
        }

        foreach(GameObject obj in existingPlaceables){
            GameObject.Destroy(obj);
        }
        existingPlaceables.Clear();
        spacesWithObjects.Clear();

        // size = new Vector3Int();
        blocks = new Block[size.x, size.y, size.z];
        heights = new int[size.x, size.z];
    }

}
