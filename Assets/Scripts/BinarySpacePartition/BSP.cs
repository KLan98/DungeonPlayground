using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BSP
{
    // The percentage of border in which splitting is not possible.
    // Prevents splits too close to room edges (5% on each side).
    private const float borderZonePercentage = 0.05f;

    /// <summary>
    /// Recursively partitions a space into smaller rooms using Binary Space Partitioning.
    /// Only leaf nodes (rooms that cannot be split further) are returned.
    /// </summary>
    /// <param name="spaceToSplit">The initial space to partition.</param>
    /// <param name="minWidth">Minimum allowed width for any room.</param>
    /// <param name="minHeight">Minimum allowed height for any room.</param>
    /// <returns>List of final leaf rooms after partitioning.</returns>
    public static List<BoundsInt> BinarySpacePartitioning(BoundsInt spaceToSplit, int minWidth, int minHeight)
    {
        Queue<BoundsInt> roomsQueue = new Queue<BoundsInt>();
        List<BoundsInt> roomsList = new List<BoundsInt>();

        roomsQueue.Enqueue(spaceToSplit);

        while (roomsQueue.Count > 0)
        {
            var room = roomsQueue.Dequeue();

            // Only attempt to split if the room meets the minimum size requirements
            if (room.size.x >= minWidth && room.size.y >= minHeight)
            {
                bool splitSucceeded = false;

                // Randomly choose whether to prioritize vertical or horizontal split
                if (UnityEngine.Random.Range(0.0f, 1.0f) < 0.5f)
                {
                    // Prioritize vertical split
                    if (room.size.x >= minWidth * 2)
                    {
                        splitSucceeded = SplitVertically(room, minWidth, roomsQueue);
                    }

                    // If previous split not successful then try to do this
                    if (!splitSucceeded && room.size.y >= minHeight * 2)
                    {
                        splitSucceeded = SplitHorizontally(room, minHeight, roomsQueue);
                    }
                }
                else
                {
                    // Prioritize horizontal split
                    if (room.size.y >= minHeight * 2)
                    {
                        splitSucceeded = SplitHorizontally(room, minHeight, roomsQueue);
                    }

                    // If previous split not successful then try to do this
                    if (!splitSucceeded && room.size.x >= minWidth * 2)
                    {
                        splitSucceeded = SplitVertically(room, minWidth, roomsQueue);
                    }
                }

                // Only add room to the final list if it could NOT be split further (leaf node)
                if (!splitSucceeded)
                {
                    roomsList.Add(room);
                }
            }
        }

        return roomsList;
    }

    /// <summary>
    /// Attempts to split a room horizontally (along the Y axis).
    /// Both resulting rooms must satisfy minHeight.
    /// </summary>
    /// <returns>True if the split succeeded and sub-rooms were enqueued.</returns>
    private static bool SplitHorizontally(BoundsInt room, int minHeight, Queue<BoundsInt> roomsQueue)
    {
        int height = room.size.y;

        // Prevent splitting too close to the borders
        int minSplitPoint = Mathf.CeilToInt(height * borderZonePercentage);
        int maxSplitPoint = Mathf.FloorToInt(height * (1f - borderZonePercentage));

        // Clamp split range so both resulting rooms satisfy minHeight
        minSplitPoint = Mathf.Max(minSplitPoint, minHeight);
        maxSplitPoint = Mathf.Min(maxSplitPoint, height - minHeight);

        // No valid split point exists
        if (minSplitPoint >= maxSplitPoint)
        {
            return false;
        }

        int splitHeight = UnityEngine.Random.Range(minSplitPoint, maxSplitPoint);

        BoundsInt room1 = new BoundsInt(room.position, new Vector3Int(room.size.x, splitHeight, 0));

        BoundsInt room2 = new BoundsInt(new Vector3Int(room.position.x, room.position.y + splitHeight, 0), new Vector3Int(room.size.x, height - splitHeight, 0));

        roomsQueue.Enqueue(room1);
        roomsQueue.Enqueue(room2);
        return true;
    }

    /// <summary>
    /// Attempts to split a room vertically (along the X axis).
    /// Both resulting rooms must satisfy minWidth.
    /// </summary>
    /// <returns>True if the split succeeded and sub-rooms were enqueued.</returns>
    private static bool SplitVertically(BoundsInt room, int minWidth, Queue<BoundsInt> roomsQueue)
    {
        int width = room.size.x;

        // Prevent splitting too close to the borders
        int minSplitPoint = Mathf.CeilToInt(width * borderZonePercentage);
        int maxSplitPoint = Mathf.FloorToInt(width * (1f - borderZonePercentage));

        // Clamp split range so both resulting rooms satisfy minWidth
        minSplitPoint = Mathf.Max(minSplitPoint, minWidth);
        maxSplitPoint = Mathf.Min(maxSplitPoint, width - minWidth);

        // No valid split point exists
        if (minSplitPoint >= maxSplitPoint)
        {
            return false;
        }

        int splitWidth = UnityEngine.Random.Range(minSplitPoint, maxSplitPoint);

        BoundsInt room1 = new BoundsInt(room.position, new Vector3Int(splitWidth, room.size.y, 0));

        BoundsInt room2 = new BoundsInt(new Vector3Int(room.position.x + splitWidth, room.position.y, 0), new Vector3Int(width - splitWidth, room.size.y, 0));

        roomsQueue.Enqueue(room1);
        roomsQueue.Enqueue(room2);
        return true;
    }
}