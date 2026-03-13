using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class RoomFirstDugeonGenerator : MonoBehaviour
{
    [SerializeField] private TilemapVisualizer tilemapVisualizer;

    [SerializeField] private Vector3Int size;

    // offset defines how far away from each other should the rooms be 
    [SerializeField] private int minWidth, minHeight, offset;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private List<BoundsInt> roomList = new List<BoundsInt>();

    private Vector3Int startPoint;
    private BoundsInt room;

    private void Awake()
    {
        startPoint = Vector3Int.CeilToInt(ViewSpaceCoverter());
        room = new BoundsInt(startPoint, size);
    }

    // automatically set the bottom left of the screen as startPosition
    private Vector3 ViewSpaceCoverter()
    {
        if (mainCamera != null)
        {
            Vector3 startPoint = mainCamera.ViewportToWorldPoint(Vector3.zero);
            return startPoint;
        }

        return Vector3.zero;
    }

    public void DestroyRooms()
    {
        tilemapVisualizer.RemoveFloorTiles(room);
    }

    public void CreateRooms()
    {
        roomList = BSP.BinarySpacePartitioning(room, minWidth, minHeight);

        // hashset for floor without the walls
        HashSet<Vector2Int> floorTilesPosition = new HashSet<Vector2Int>();
        floorTilesPosition = GenerateFloor(roomList);

        tilemapVisualizer.DrawFloorTiles(floorTilesPosition);
    }

    // room consists of floor and walls
    private HashSet<Vector2Int> GenerateFloor(List<BoundsInt> roomsList)
    {
        HashSet<Vector2Int> floorTilesPosition = new HashSet<Vector2Int>();

        foreach(var room in roomsList)
        {
            for (int col = offset; col < room.size.x - offset; col++)
            {
                for (int row = offset; row < room.size.y - offset; row++)
                {
                    Vector2Int nextTilePosition = (Vector2Int)room.min + new Vector2Int(col, row);
                    floorTilesPosition.Add(nextTilePosition);
                }
            }
        }

        return floorTilesPosition;
    }
}