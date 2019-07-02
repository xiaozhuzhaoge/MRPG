using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BagBlock : MonoBehaviour {

    public BlockType type = BlockType.Bag;
    public enum BlockType
    {
        Bag,
        CharacterBag
    }
}
