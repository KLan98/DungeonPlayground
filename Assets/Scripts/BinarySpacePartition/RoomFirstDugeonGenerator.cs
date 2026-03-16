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
    [SerializeField] private int minWidth, minHeight, minOffset, maxOffset;
    [SerializeField] private List<BoundsInt> roomsList = new List<BoundsInt>();

    [Header("Corridor settings")]
    [SerializeField] private Vector3Int corridorSize;
    [SerializeField] private int corridorMinWidth, corridorMinHeight;
    [SerializeField] private List<BoundsInt> corridorsList = new List<BoundsInt>();

    private List<Vector2Int> listOfCenters = new List<Vector2Int>();
    private Vector3Int startPoint;
    private BoundsInt room;
    private int offset;

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
        roomsList = BSP.BinarySpacePartitioning(room, minWidth, minHeight);

        // hashset for floor without the walls
        HashSet<Vector2Int> floorTilesPosition = new HashSet<Vector2Int>();
        floorTilesPosition = GenerateFloor(roomsList);

        listOfCenters = GetRoomsCenter(roomsList);

        HashSet<Vector2Int> corridors = BuildCorridorsBetweenRooms(listOfCenters);
        floorTilesPosition.UnionWith(corridors);

        tilemapVisualizer.DrawFloorTiles(floorTilesPosition);
    }

    // room consists of floor and walls
    private HashSet<Vector2Int> GenerateFloor(List<BoundsInt> roomsList)
    {
        HashSet<Vector2Int> floorTilesPosition = new HashSet<Vector2Int>();

        foreach(var room in roomsList)
        {
            offset = UnityEngine.Random.Range(minOffset, maxOffset);

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
    private HashSet<Vector2Int> BuildCorridorsBetweenRooms(List<Vector2Int> listOfCenters)
    {
        HashSet<Vector2Int> allCorridorTiles = new HashSet<Vector2Int>();
        
        // pick a random center as starting point
        Vector2Int currentCenter = listOfCenters[UnityEngine.Random.Range(0, listOfCenters.Count)];
        
        // currentCenter is not a valid target to find nearest center
        listOfCenters.Remove(currentCenter);

        while (listOfCenters.Count > 0)
        {
            Vector2Int nearestCenter = FindNearestCenter(currentCenter, listOfCenters);
            
            // nearest center is not a valid target anymore, remove it from list
            listOfCenters.Remove(nearestCenter);
            HashSet<Vector2Int> newTilesPosition = CreateCorridor(currentCenter, nearestCenter);
            
            // the new currentCenter is the newest
            currentCenter = nearestCenter;

            // combine the available corridorTilesPosition with newTilesPosition
            allCorridorTiles.UnionWith(newTilesPosition);
        }

        return allCorridorTiles;
    }

    /// <summary>
    /// Creates an L-shaped corridor between two room centers by walking tile by tile.
    /// First moves vertically until the destination's Y is reached, 
    /// then moves horizontally until the destination's X is reached.
    /// </summary>
    /// <param name="currentCenter">The starting position of the corridor.</param>
    /// <param name="destination">The end position of the corridor.</param>
    /// <returns>A HashSet of tile positions representing the corridor.</returns>
    private HashSet<Vector2Int> CreateCorridor(Vector2Int currentCenter, Vector2Int destination)
    {
        HashSet<Vector2Int> corridor = new HashSet<Vector2Int>();
        var position = currentCenter;
        corridor.Add(position);
        while (position.y != destination.y)
        {
            if (destination.y > position.y)
            {
                position += Vector2Int.up;
            }

            else if (destination.y < position.y)
            {
                position += Vector2Int.down;
            }
            corridor.Add(position);
        }

        while (position.x != destination.x)
        {
            if (destination.x > position.x)
            {
                position += Vector2Int.right;
            }

            else if (destination.x < position.x)
            {
                position += Vector2Int.left;
            }
            corridor.Add(position);
        }

        return corridor;
    }

    //private Vector2Int FindNearestCenter(Vector2Int currentCenter, List<Vector2Int> listOfCenters)
    //{
    //    Dictionary<float, Vector2Int> roomsInfo = new Dictionary<float, Vector2Int>();  

    //    // currentCenter should not be included in calculation, since minKey will return 0
    //    listOfCenters.Remove(currentCenter); 

    //    foreach (var center in listOfCenters)
    //    {
    //        float distance = Vector2.Distance(currentCenter, center);
    //        roomsInfo.Add(distance, center);
    //    }

    //    float minKey = roomsInfo.Keys.Min();
        
    //    Vector2Int nearestCenter = roomsInfo[minKey];  

    //    return nearestCenter;
    //}

    // linear search O(N) 
    // find the nearest center from a given center, the listOfCenters contains all possible centers
    // current center is const in this method
    private Vector2Int FindNearestCenter(Vector2Int currentCenter, List<Vector2Int> listOfCenters)
    {
        // assume the nearest center is at index 0
        Vector2Int nearestCenter = listOfCenters[0];

        // calculate min distance of this nearest center -> this is the value used for comparing
        float minDistance = Vector2.Distance(currentCenter, nearestCenter);

        foreach (var center in listOfCenters)
        {
            float distance = Vector2.Distance(currentCenter, center);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestCenter = center;
            }
        }

        return nearestCenter;
    }

    private List<Vector2Int> GetRoomsCenter(List<BoundsInt> roomsList)
    {
        List<Vector2Int> listOfCenters = new List<Vector2Int>();

        if (roomsList.Count > 0)
        {
            foreach (var room in roomsList)
            {
                Vector2Int center = new Vector2Int(Mathf.RoundToInt(room.center.x), Mathf.RoundToInt(room.center.y));
                listOfCenters.Add(center);
            }
        }

        else
        {
            return null;
        }

        return listOfCenters;
    }
}     