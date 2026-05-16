using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public class DungeonTilesDatabase
{
    [SerializeField] private TileBase[] groundTiles;
    [SerializeField] private TileBase[] edgeTiles;
    [SerializeField] private TileBase[] waterTiles;

    public DungeonTilesDatabase()
    {

    }

    public void LoadTerrainTiles()
    {
        groundTiles = new TileBase[]{
            Resources.Load<TileBase>("Dungeon/Terrain/TerrainPalette/Tilemap_color3_9"),
            Resources.Load<TileBase>("Dungeon/Terrain/TerrainPalette/Tilemap_color4_9"),
        };

        edgeTiles = new TileBase[]{
            Resources.Load<TileBase>("Dungeon/Terrain/TerrainPalette/Tilemap_color3_0"),
            Resources.Load<TileBase>("Dungeon/Terrain/TerrainPalette/Tilemap_color3_1"),
            Resources.Load<TileBase>("Dungeon/Terrain/TerrainPalette/Tilemap_color3_2"),
            Resources.Load<TileBase>("Dungeon/Terrain/TerrainPalette/Tilemap_color3_8"),
            Resources.Load<TileBase>("Dungeon/Terrain/TerrainPalette/Tilemap_color3_9"),
            Resources.Load<TileBase>("Dungeon/Terrain/TerrainPalette/Tilemap_color3_10"),
            Resources.Load<TileBase>("Dungeon/Terrain/TerrainPalette/Tilemap_color3_17"),
            Resources.Load<TileBase>("Dungeon/Terrain/TerrainPalette/Tilemap_color3_21"),
        };
    }
}
