using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RoomFirstDugeonGenerator : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private TilemapVisualizer tilemapVisualizer;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private DungeonGrid dungeonGrid;
    [SerializeField] private Grid grid;

    [Header("Arena settings")]
    [SerializeField] private Vector3Int arenaSize; // the size of tileMap 
    // offset defines how far away from each other should the rooms be 
    [SerializeField] private int offset;
    [SerializeField] private int minWidth, minHeight;
    [SerializeField] private int maxWidth, maxHeight;
    [SerializeField] private List<BoundsInt> roomsList = new List<BoundsInt>();

    //------------------------------PRIVATE FIELDS-----------------------------------------
    private List<Vector2Int> listOfCenters = new List<Vector2Int>();
    private HashSet<Vector2Int> tilesPosition = new HashSet<Vector2Int>();
    private HashSet<Vector2Int> corridors = new HashSet<Vector2Int>();
    private Vector3Int arenaStartPoint;
    private BoundsInt arena;
    private Client client;

    //------------------------------PRIVATE PROPERTIES-----------------------------------------
    //private Vector2 dimension
    //{
    //    get { return new Vector2(arenaSize.x, arenaSize.y); }
    //}

    private void Awake()
    {
        arenaStartPoint = Vector3Int.CeilToInt(ViewSpaceCoverter());

        CheckArenaSettings();

        if (arenaSize != Vector3Int.zero)
        { 
            arena = new BoundsInt(arenaStartPoint, arenaSize);
        }
    }

    public void DeleteDungeon()
    {
        tilemapVisualizer.ClearTiles(arena);
    }

    public void CreateDungeon()
    {
        CheckArenaSettings();
        CreateRooms();
        CreateCorridors();
        tilemapVisualizer.DrawTiles(tilesPosition);

        if (dungeonGrid != null)
        {
            foreach (var tilePosition in tilesPosition)
            {
                // tilePosition needs correction to be the center of the cell, that's why the + 0.5f 
                // cellSize should be 1 not sure why the correct version is 0.5. LAN_TODO: find the root cause
                client = dungeonGrid.spatialHashGrid.NewClient(new Vector2(tilePosition.x + 0.5f, tilePosition.y + 0.5f), grid.cellSize/2, "Tile" + $"{tilePosition}");
            }
        }
    }

    private void CreateRooms()
    {
        roomsList = BSP.BinarySpacePartitioning(arena, minWidth, minHeight);
        RandomizeRoomSize(roomsList);
        tilesPosition = GenerateFloor(roomsList);
    }
    
    private void CreateCorridors()
    {
        listOfCenters = GetRoomsCenter(roomsList);

        if (listOfCenters.Count > 1)
        {
            corridors = BuildCorridorsBetweenRooms(listOfCenters);
            tilesPosition.UnionWith(corridors);
        }

        else if (listOfCenters.Count <= 1)
        {
            Debug.LogWarning($"listOfCenters = {listOfCenters.Count}, unable to create corridors");
            return;
        }
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

    // room consists of floor and walls
    private HashSet<Vector2Int> GenerateFloor(List<BoundsInt> roomsList)
    {
        HashSet<Vector2Int> roomTiles = new HashSet<Vector2Int>();

        foreach(var room in roomsList)
        {
            for (int col = offset; col < room.size.x - offset; col++)
            {
                for (int row = offset; row < room.size.y - offset; row++)
                {
                    // during interation room.min is const
                    // only col and row change
                    Vector2Int nextTilePosition = (Vector2Int)room.min + new Vector2Int(col, row);
                    roomTiles.Add(nextTilePosition);
                }
            }
        }

        return roomTiles;
    }

    /// <summary>
    /// Creates an L-shaped corridor between two room centers by walking tile by tile.
    /// First moves vertically until the destination's Y is reached, 
    /// then moves horizontally until the destination's X is reached.
    /// </summary>
    /// <param name="startingPoint">The starting position of the corridor.</param>
    /// <param name="destination">The end position of the corridor.</param>
    /// <returns>A HashSet of tile positions representing the corridor.</returns>
    private HashSet<Vector2Int> CreateCorridor(Vector2Int startingPoint, Vector2Int destination)
    {
        HashSet<Vector2Int> corridor = new HashSet<Vector2Int>();
        corridor.Add(startingPoint);
        while (startingPoint.y != destination.y)
        {
            if (destination.y > startingPoint.y)
            {
                startingPoint += Vector2Int.up;
            }

            else if (destination.y < startingPoint.y)
            {
                startingPoint += Vector2Int.down;
            }
            corridor.Add(startingPoint);
        }

        while (startingPoint.x != destination.x)
        {
            if (destination.x > startingPoint.x)
            {
                startingPoint += Vector2Int.right;
            }

            else if (destination.x < startingPoint.x)
            {
                startingPoint += Vector2Int.left;
            }
            corridor.Add(startingPoint);
        }

        return corridor;
    }

    private void RandomizeRoomSize(List<BoundsInt> roomsList)
    {
        for (int i = 0; i < roomsList.Count; i++)
        {
            var room = roomsList[i];    
            room.size = new Vector3Int(UnityEngine.Random.Range(minWidth, maxWidth), UnityEngine.Random.Range(minHeight, maxHeight), 0);
            roomsList[i] = room;
        }
    }

    private List<Vector2Int> GetRoomsCenter(List<BoundsInt> roomsList)
    {
        List<Vector2Int> listOfCenters = new List<Vector2Int>();

        if (roomsList.Count > 0)
        {
            foreach (var room in roomsList)
            {
                Vector2Int center = new Vector2Int(Mathf.FloorToInt(room.center.x), Mathf.FloorToInt(room.center.y));
                listOfCenters.Add(center);
            }
        }

        else
        {
            return null;
        }

        return listOfCenters;
    }

    private HashSet<Vector2Int> BuildCorridorsBetweenRooms(List<Vector2Int> listOfCenters)
    {
        HashSet<Vector2Int> allCorridorTiles = new HashSet<Vector2Int>();

        foreach (Corridor corridor in PrimAlgorithm.Prim(listOfCenters))
        {
            allCorridorTiles.UnionWith(CreateCorridor(corridor.startPoint, corridor.endPoint));
        }

        return allCorridorTiles;
    }

    private void CheckArenaSettings()
    {
        if (arenaSize == Vector3Int.zero || arenaSize.x < minWidth || arenaSize.y < minHeight)
        {
            throw new ArgumentOutOfRangeException(nameof(arenaSize), "Arena size not in allowed range");
        }

        if (offset <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(offset), "Offset not in allowed range");
        }

        if (minWidth > arenaSize.x || minWidth <= 0)
        { 
            throw new ArgumentOutOfRangeException(nameof(minWidth), "minWidth not in allowed range");
        }

        if (minHeight > arenaSize.y || minHeight <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(minHeight), "minHeight not in allowed range");
        }

        if (maxWidth > arenaSize.x || maxWidth <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(maxWidth), "maxWidth not in allowed range");
        }

        if (maxHeight > arenaSize.y || maxHeight <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(maxHeight), "maxHeight not in allowed range");
        }

        if (maxHeight <= minHeight || maxWidth <= minWidth)
        {
            throw new ArgumentOutOfRangeException("Inconsistency in width and height settings");
        }
    }
}     