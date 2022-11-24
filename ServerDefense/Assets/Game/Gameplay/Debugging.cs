using UnityEngine;
using UnityEngine.Tilemaps;

// Tile that repeats two sprites in checkerboard pattern
[CreateAssetMenu(fileName = "DebugTile", menuName = "DebugTile")]
public class Debugging : TileBase
{
    public Sprite spriteA;
    public Sprite spriteB;

    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        bool evenCell = Mathf.Abs(position.y + position.x) % 2 > 0;
        tileData.sprite = evenCell ? spriteA : spriteB;
    }
}