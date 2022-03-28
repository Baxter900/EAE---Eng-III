using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BlockLibrary", menuName = "Blocks/BlockLibrary", order = 1)]
public class BlockLibrary : ScriptableObject{
    public BlockType[] types;

    public int GetIndex(BlockType targetType){
        for(int i = 0; i < types.Length; i++){
            if(types[i] == targetType){
                return i;
            }
        }
        return -1;
    }

    public BlockType GetType(int i){
        return types[i];
    }
}
