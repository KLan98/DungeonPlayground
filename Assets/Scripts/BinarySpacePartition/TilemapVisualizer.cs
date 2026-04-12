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

    public void ColorTileByDistance(Vector2 position, int distance, int maxDistance)
    {
        var cellPosition = map.WorldToCell((Vector3)position);
        map.SetTileFlags(cellPosition, TileFlags.None); // required to allow color override

        switch (distance)
        {

            case 0:
                map.SetColor(cellPosition, Color.yellow);
                break;
            case 1:
                map.SetColor(cellPosition, Color.green);
                break;
            case 2:
                map.SetColor(cellPosition, Color.cyan);
                break;
            case 3:
                map.SetColor(cellPosition, Color.blue);
                break;
            case 4:
                map.SetColor(cellPosition, Color.red);
                break;
            case 5:
                map.SetColor(cellPosition, Color.black);
                break;
            default:
                map.SetColor(cellPosition, Color.black);
                break;
        }
    }
}