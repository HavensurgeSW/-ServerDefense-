using UnityEngine;
using UnityEngine.Tilemaps;

using ServerDefense.Gameplay.Gameplay.Modules.Terminal;

namespace ServerDefense.Gameplay.Gameplay.Modules.Map
{
    public class MapHandler : MonoBehaviour
    {
        [Header("Main Configuration")]
        [SerializeField] private Tilemap tilemap = null;
        [SerializeField] private TileBase selectedLocation = null;
        [SerializeField] private TileBase defaultLocation = null;

        [Header("Locations Configuration")]
        [SerializeField] private Location[] locations = null;

        [Header("Visual Configuration")]
        [SerializeField] private MapHandlerUI mapHandlerUI = null;

        [Header("Terminal Responses Configuration")]
        [SerializeField] private TerminalResponseSO invalidLocationResponse = null;

        private Location currentLocation = null;

        public Location CURRENT_LOCATION { get => currentLocation; }
        public Location[] LOCATIONS { get => locations; }

        public void Init()
        {
            currentLocation = null;
            mapHandlerUI.Init();
        }

        public void SetCurrentLocation(Location location, bool showPopup = true)
        {
            if (currentLocation != null && currentLocation != location)
            {
                currentLocation.ToggleSelected(false);
                SetTileToDefault(currentLocation.transform.position);
            }

            currentLocation = location;

            if (currentLocation)
            {
                currentLocation.ToggleSelected(true);
                SetTileToSelected(location.transform.position);
            }

            if (showPopup)
            {
                mapHandlerUI.GeneratePopUp(location);
            }
        }

        public bool GetIsCurrentLocationAvailable()
        {
            return currentLocation != null && currentLocation.GetAvailability();
        }

        private Vector3Int WorldToCellId(Vector3 worldPosition)
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

        private void SetTileToDefault(Vector3 worldPosition)
        {
            tilemap.SetTile(WorldToCellId(worldPosition), defaultLocation);
        }

        private void SetTileToSelected(Vector3 worldPosition)
        {
            tilemap.SetTile(WorldToCellId(worldPosition), selectedLocation);
        }

        public TerminalResponseSO GetInvalidLocationResponse()
        {
            return invalidLocationResponse;
        }

        public void GeneratePopUp(Location location)
        {
            mapHandlerUI.GeneratePopUp(location);
        }

        public void ClearAllPopUps()
        {
            mapHandlerUI.ClearAllPopUps();
        }
    }
}