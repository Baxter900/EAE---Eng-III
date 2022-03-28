using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BlockType", menuName = "Blocks/BlockType", order = 1)]
public class BlockType : ScriptableObject{
    public string typeName;
    public Material material;
}
