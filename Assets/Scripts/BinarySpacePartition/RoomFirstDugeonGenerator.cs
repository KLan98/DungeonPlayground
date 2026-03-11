using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomFirstDugeonGenerator : MonoBehaviour
{
    [SerializeField] private TilemapVisualizer tilemapVisualizer;

    [SerializeField] private Vector3Int size;

    [SerializeField] private int minWidth, minHeight, offset;
    [SerializeField] private Camera mainCamera;

    private void Start()
    {
        CreateRooms();
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

    private void CreateRooms()
    {
        Vector3Int startPoint = Vector3Int.CeilToInt(ViewSpaceCoverter());
        var roomList = BSP.BinarySpacePartitioning(new BoundsInt(startPoint, size), minWidth, minHeight);

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
                    Vector2Int nextTilePosition = (Vector2Int)room.min + new Vector2Int(row, col);
                    floorTilesPosition.Add(nextTilePosition);
                }
            }
        }

        return floorTilesPosition;
    }
}
