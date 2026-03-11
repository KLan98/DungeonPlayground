using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapVisualizer : MonoBehaviour
{
    [SerializeField] Tilemap map;
    [SerializeField] TileBase tileBase;

    public void DrawFloorTiles(IEnumerable<Vector2Int> floorPositions)
    {
        DrawTiles(floorPositions, map, tileBase);
    }

    private void DrawTiles(IEnumerable<Vector2Int> positions, Tilemap map, TileBase tile)
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
