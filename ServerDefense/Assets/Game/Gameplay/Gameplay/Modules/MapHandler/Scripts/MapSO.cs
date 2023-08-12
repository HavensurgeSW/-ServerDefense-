using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace ServerDefense.Gameplay.Gameplay.Modules.Map
{
    public class MapSO : ScriptableObject
    {
        private List<CachedTile> tiles = null; // TODO: Replace with stronger data type

        public void Configure(List<CachedTile> tiles)
        {
            this.tiles = tiles;
        }
    }

    public class CachedTile
    {
        [SerializeField] private MapTile mapTile = null;
        [SerializeField] private Vector3Int gridPosition = Vector3Int.zero;

        public MapTile MapTile { get => mapTile; }
        public Vector3Int GridPosition { get => gridPosition; }

        public CachedTile(MapTile mapTile, Vector3Int gridPosition)
        {
            this.mapTile = mapTile;
            this.gridPosition = gridPosition;
        }
    }
}