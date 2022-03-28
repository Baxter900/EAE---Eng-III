using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CollisionSystem{
    private static HashSet<GridTransform> transforms = new HashSet<GridTransform>();

    public static void Register(GridTransform transform){
        transforms.Add(transform);
    }

    public static void Unregister(GridTransform transform){
        transforms.Remove(transform);
    }

    public static bool DoesCollide(GridTransform transform, Vector3Int newPosition){
        foreach(GridTransform other in transforms){
            if(other != transform){
                foreach(Vector3Int thisPos in transform.GetTilePositionIterator(newPosition, transform.dimensions)){
                    foreach(Vector3Int otherPos in other){
                        if(thisPos == otherPos){
                            return true;
                        }
                    }
                }
            }
        }
        return false;
    }

    public static GridTransform GetObjectAt(Vector3Int position){
        foreach(GridTransform gridTransform in transforms){
            foreach(Vector3Int otherPosition in gridTransform){
                if(position == otherPosition){
                    return gridTransform;
                }
            }
        }
        return null;
    }
}
