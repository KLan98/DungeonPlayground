using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomFirstDugeonGenerator : MonoBehaviour
{
    [SerializeField] private TilemapVisualizer tilemapVisualizer;

    [SerializeField] private Vector3Int startPosition, size;

    [SerializeField] private int minWidth, minHeight, offset;

    private void Start()
    {
        CreateRooms();
    }

    private void CreateRooms()
    {
        var roomList = BSP.BinarySpacePartitioning(new BoundsInt(startPosition, size), minWidth, minHeight);

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
