using UnityEngine;
using UnityEngine.Tilemaps;

public class MapHandler : MonoBehaviour
{
    [Header("Main Configuration")]
    [SerializeField] private Tilemap tilemap = null;
    [SerializeField] private TileBase selectedLocation = null;
    [SerializeField] private TileBase defaultLocation = null;
    
    [Header("Terminal Responses Configuration")]
    [SerializeField] private TerminalResponseSO invalidLocationResponse = null;

    private Location currentLocation = null;

    public Location CURRENT_LOCATION { get => currentLocation; }

    public void Init()
    {
        currentLocation = null;
    }

    public void SetCurrentLocation(Location location)
    {
        currentLocation = location;
    }

    public bool GetIsCurrentLocationAvailable()
    {
        return currentLocation != null && currentLocation.GetAvailability();
    }

    public Vector3Int WorldToCellId(Vector3 worldPosition)
    {
        return tilemap.WorldToCell(worldPosition);
    }

    public void SetTileToDefault(Vector3Int cellId)
    {
        tilemap.SetTile(cellId, defaultLocation);
    }

    public void SetTileToSelected(Vector3Int cellId)
    {
        tilemap.SetTile(cellId, selectedLocation);
    }

    public void SetTileToDefault(Vector3 worldPosition)
    {
        tilemap.SetTile(WorldToCellId(worldPosition), defaultLocation);
    }

    public void SetTileToSelected(Vector3 worldPosition)
    {
        tilemap.SetTile(WorldToCellId(worldPosition), selectedLocation);
    }

    public TerminalResponseSO GetInvalidLocationResponse()
    {
        return invalidLocationResponse;
    }
}
