using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BSP
{
    private const float borderZonePercentage = 0.05f; // the percentage of border in which splitting is not possible, used to prevent splitting at borders of a room -> 5% at the beginning of the room is not possible to split, 5% at the end section is also not possible to split

    // parameters:
    // spaceToSplit: rooms or room to be splitted
    // minWidth: the minimum allowed width to split
    // minHeigth: the minimum allowed heigth split
    public static List<BoundsInt> BinarySpacePartitioning(BoundsInt spaceToSplit, int minWidth, int minHeight)
    {
        Queue<BoundsInt> roomsQueue = new Queue<BoundsInt>(); // queue of rooms to be splitted
        List<BoundsInt> roomsList = new List<BoundsInt>(); // list of rooms after splitting

        // enqueu the queue
        roomsQueue.Enqueue(spaceToSplit);
        
        // while there are still rooms to split
        while (roomsQueue.Count > 0)
        {
            var room = roomsQueue.Dequeue();

            // if the room size is greater than the allowed min width/height 
            if (room.size.x >= minWidth && room.size.y >= minHeight)
            {
                // when to split horizontally?
                // current implementation if room size.y is greater than x2 of minHeight
                // when to split vertically?
                // current implementation if room size.x is greater than x2 of minWidth
                
                // prioritize vertical split
                if (UnityEngine.Random.Range(0.0f, 1.0f) < 0.5f)
                {
                    if (room.size.x >= minWidth * 2)
                    {
                        SplitVertically(room, minWidth, roomsQueue);
                    }

                    else if (room.size.y >= minHeight *2)
                    {
                        SplitHorizontally(room, minWidth, roomsQueue);
                    }

                    else
                    {
                        // room cannot be splitted, add to room list
                    }
                }

                // prioritize horizontal split
                else
                {
                    if (room.size.y >= minHeight * 2)
                    {
                        SplitHorizontally(room, minWidth, roomsQueue);
                    }

                    else if (room.size.x >= minWidth * 2)
                    {
                        SplitVertically(room, minWidth, roomsQueue);
                    }

                    else
                    {
                        // room cannot be splitted, add to room list
                    }
                }

                // as long as a room satisfies the minWidth and minHeight criteria, add it to roomsList
                roomsList.Add(room);
            }
        }

        return roomsList;
    }

    private static void SplitHorizontally(BoundsInt room, int minHeight, Queue<BoundsInt> roomsQueue)
    {
        int height = room.size.y;

        // prevent splitting at borders
        float forbiddenZone = height * borderZonePercentage;
        float maxAllowedZone = height - forbiddenZone; // maxInclusive
        float minAllowedZone = forbiddenZone; // minInclusive

        // careful with this typecast!
        int splittedHeight = (int)UnityEngine.Random.Range(minAllowedZone, maxAllowedZone);

        // if after splitting the height of both rooms > minHeight then allow the operation
        if (splittedHeight - minHeight > 0)
        {
            // after splitting there will be 2 rooms 
            // BoundsInt(vec3int pos, vec3int size)
            BoundsInt room1 = new BoundsInt(room.position, new Vector3Int(room.size.x, splittedHeight));
            BoundsInt room2 = new BoundsInt(new Vector3Int(room.position.x, room.position.y + splittedHeight),
            new Vector3Int(room.size.x, height - splittedHeight));

            roomsQueue.Enqueue(room1);
            roomsQueue.Enqueue(room2);
        }
    }

    private static void SplitVertically(BoundsInt room, int minWidth, Queue<BoundsInt> roomsQueue)
    {
        int width = room.size.x;

        // prevent splitting at borders
        float forbiddenZone = width * borderZonePercentage;
        float maxAllowedZone = width - forbiddenZone; // maxInclusive
        float minAllowedZone = forbiddenZone; // minInclusive

        // careful with this typecast!
        int splittedWidth = (int)UnityEngine.Random.Range(minAllowedZone, maxAllowedZone);

        // if after splitting the width of both rooms > minWidth then allow the operation
        if (splittedWidth -  minWidth > 0)
        {
            // after splitting there will be 2 rooms 
            // BoundsInt(vec3int pos, vec3int size)
            BoundsInt room1 = new BoundsInt(room.position, new Vector3Int(splittedWidth, room.size.y));
            BoundsInt room2 = new BoundsInt(new Vector3Int(room.position.x + splittedWidth, room.position.y),
            new Vector3Int(width - splittedWidth, room.size.y));

            roomsQueue.Enqueue(room1);
            roomsQueue.Enqueue(room2);
        }
    }
}
