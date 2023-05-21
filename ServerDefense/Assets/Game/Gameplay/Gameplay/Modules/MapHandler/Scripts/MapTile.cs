using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Tilemaps;

public enum TYLE_TYPE
{
    NONE = -1,
    GROUND = 0,
    TOWER_POSITION = 1,
    PATH = 2
}

[CreateAssetMenu(fileName = "MapTile", menuName = "ScriptableObjects/Map/Tiles/MapTile")]
public class MapTile : Tile //this sucks but i really got no other clue as to how to identify the tile type
{
    [SerializeField] private TYLE_TYPE type = TYLE_TYPE.NONE;
}