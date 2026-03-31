using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapVisualizer : MonoBehaviour
{
    [SerializeField] private Tilemap map;
    [SerializeField] private TileBase tileBase;

    public void DrawTiles(IEnumerable<Vector2Int> floorPositions)
    {
        DrawMultipleTiles(floorPositions, map, tileBase);
    }

    public void ClearTiles(BoundsInt arena)
    {
        map.DeleteCells(arena.position, arena.size);
    }

    private void DrawMultipleTiles(IEnumerable<Vector2Int> positions, Tilemap map, TileBase tile)
    {
        foreach(var position in positions)
        {
            DrawSingleTile(position, map, tile);
        }
    }

    private void DrawSingleTile(Vector2Int position, Tilemap map, TileBase tile)
    {
        var cellPostion = map.WorldToCell((Vector3Int)position);
        map.SetTile(cellPostion, tile);
    }
}