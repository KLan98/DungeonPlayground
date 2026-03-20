using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class RoomFirstDugeonGenerator : MonoBehaviour
{
    [SerializeField] private TilemapVisualizer tilemapVisualizer;
    [SerializeField] private Camera mainCamera;

    [Header("Room settings")]
    [SerializeField] private Vector3Int size;
    // offset defines how far away from each other should the rooms be 
    [SerializeField] private int offset;
    [SerializeField] private int minWidth, minHeight;
    [SerializeField] private int maxWidth, maxHeight;
    [SerializeField] private List<BoundsInt> roomsList = new List<BoundsInt>();
    [SerializeField] private List<BoundsInt> roomInorderTraversal = new List<BoundsInt>();

    private List<Vector2Int> listOfCenters = new List<Vector2Int>();
    private HashSet<Vector2Int> tilesPosition = new HashSet<Vector2Int>();
    private HashSet<Vector2Int> corridors = new HashSet<Vector2Int>();
    private Vector3Int startPoint;
    private BoundsInt arena;

    private void Awake()
    {
        startPoint = Vector3Int.CeilToInt(ViewSpaceCoverter());
        arena = new BoundsInt(startPoint, size);
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

    public void DeleteDungeon()
    {
        tilemapVisualizer.ClearTiles(arena);
        corridors.Clear();
        tilesPosition.Clear();
        roomsList.Clear();
        listOfCenters.Clear();
        roomInorderTraversal.Clear();
    }

    public void CreateRooms()
    {
        roomsList = BSP.BinarySpacePartitioning(arena, minWidth, minHeight);
        RandomizeRoomSize(roomsList);
        // hashset for floor without the walls
        tilesPosition = GenerateFloor(roomsList);

        CreateCorridors();
    }

    private void CreateCorridors()
    {
        //BinaryTreeCreation(roomsList);

        //roomInorderTraversal = BinaryTree.InorderTraversal(BinaryTree.RootNode);
        listOfCenters = GetRoomsCenter(roomsList);
        corridors = BuildCorridorsBetweenRooms(listOfCenters);
        
        tilesPosition.UnionWith(corridors);
        tilemapVisualizer.DrawFloorTiles(tilesPosition);
    }

    private void BinaryTreeCreation(List<BoundsInt> roomsList)
    {
        BinaryTree.Reset(); // clear stale state before rebuilding
        foreach (var room in roomsList)
        {
            BinaryTree.Insert(room);
        }
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
                    // during interation room.min is const
                    // only col and row change
                    Vector2Int nextTilePosition = (Vector2Int)room.min + new Vector2Int(col, row);
                    floorTilesPosition.Add(nextTilePosition);
                }
            }
        }

        return floorTilesPosition;
    }

    /// <summary>
    /// Builds corridors between all rooms by connecting their centers using a nearest-neighbor approach.
    /// Starts from a random room center, repeatedly finds the nearest unvisited center,
    /// and creates a corridor to it until all rooms are connected.
    /// </summary>
    /// <param name="listOfCenters">The list of room centers to connect. This list is modified during execution.</param>
    /// <returns>A HashSet of tile positions representing all corridor tiles.</returns>
    //private HashSet<Vector2Int> BuildCorridorsBetweenRooms(List<Vector2Int> listOfCenters)
    //{
    //    HashSet<Vector2Int> allCorridorTiles = new HashSet<Vector2Int>();
        
    //    // pick center at index 0 as starting point
    //    Vector2Int currentCenter = listOfCenters[0];

    //    for (int i = 0; i < listOfCenters.Count - 1; i++)
    //    {
    //        Vector2Int nextCenter = listOfCenters[i + 1];

    //        HashSet<Vector2Int> newTilesPosition = CreateCorridor(currentCenter, nextCenter);

    //        currentCenter = nextCenter;

    //        allCorridorTiles.UnionWith(newTilesPosition);
    //    }

    //    return allCorridorTiles;
    //}

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

    private List<(Vector2Int, Vector2Int)> BuildMST(List<Vector2Int> centers)
    {
        var inMST = new HashSet<Vector2Int> { centers[0] };
        var edges = new List<(Vector2Int, Vector2Int)>();

        while (inMST.Count < centers.Count)
        {
            float bestDist = float.MaxValue;
            Vector2Int bestA = default, bestB = default;

            foreach (var a in inMST)
            {
                foreach (var b in centers)
                {
                    if (inMST.Contains(b)) continue;
                    float d = Vector2Int.Distance(a, b);
                    if (d < bestDist) { bestDist = d; bestA = a; bestB = b; }
                }
            }

            inMST.Add(bestB);
            edges.Add((bestA, bestB));
        }

        return edges;
    }

    private HashSet<Vector2Int> BuildCorridorsBetweenRooms(List<Vector2Int> listOfCenters)
    {
        HashSet<Vector2Int> allCorridorTiles = new HashSet<Vector2Int>();

        foreach (var (a, b) in BuildMST(listOfCenters))
        {
            allCorridorTiles.UnionWith(CreateCorridor(a, b));
        }

        return allCorridorTiles;
    }
}     