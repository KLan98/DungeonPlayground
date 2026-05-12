using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapVisualizer : MonoBehaviour
{
    [SerializeField] private Tilemap map;
    [SerializeField] private TileBase tileBase;
    private readonly Color32 red = new Color32(0xFF, 0, 0, 0xFF);
    private readonly Color32 orange = new Color32(0xFF, 0x7F, 0, 0xFF);
    private readonly Color32 yellow = new Color32(0xFF, 0xFF, 0, 0xFF);
    private readonly Color32 green = new Color32(0, 0xFF, 0, 0xFF);
    private readonly Color32 blue = new Color32(0, 0, 0xFF, 0xFF);
    private readonly Color32 indigo = new Color32(0x4B, 0, 0x82, 0xFF);
    private readonly Color32 violet = new Color32(0x94, 0, 0xD3, 0xFF);
    private readonly Color32 cyan = new Color32(0, 0xFF, 0xFF, 0xFF);
    private readonly Color32 azure = new Color32(0, 0x7F, 0xFF, 0xFF);
    private readonly Color32 lime = new Color32(0x7F, 0xFF, 0, 0xFF);
    private static TilemapVisualizer instance;

    private void Awake()
    {
        if (instance != null && instance == this)
        {
            return;
        }

        instance = this;
    }

    public static TilemapVisualizer GetInstance()
    {
        return instance;
    }

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

    public void ColorTileByDistance(Vector2 position, int distance)
    {
        var cellPosition = map.WorldToCell((Vector3)position);
        map.SetTileFlags(cellPosition, TileFlags.None); // required to allow color override

        switch (distance)
        {
            case 1:
                map.SetColor(cellPosition, red);
                break;
            case 2:
                map.SetColor(cellPosition, orange);
                break;
            case 3:
                map.SetColor(cellPosition, yellow);
                break;
            case 4:
                map.SetColor(cellPosition, green);
                break;
            case 5:
                map.SetColor(cellPosition, cyan);
                break;
            case 6:
                map.SetColor(cellPosition, azure);
                break;
            case 7:
                map.SetColor(cellPosition, blue);
                break;
            case 8:
                map.SetColor(cellPosition, indigo);
                break;
            default:
                map.SetColor(cellPosition, Color.black);
                break;
        }
    }

    public void VisualizePath(Vector2 position)
    {
        var cellPosition = map.WorldToCell((Vector3)position);
        map.SetTileFlags(cellPosition, TileFlags.None); // required to allow color override
        map.SetColor(cellPosition, Color.black);
    }

    public void ResetMapColor(Vector2 position)
    {
        var cellPosition = map.WorldToCell((Vector3)position);
        map.SetTileFlags(cellPosition, TileFlags.None); // required to allow color override
        map.SetColor(cellPosition, Color.white);
    }
}