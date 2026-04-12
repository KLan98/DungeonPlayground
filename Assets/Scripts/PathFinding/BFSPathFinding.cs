using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PathFinding
{
    public class BFSPathFinding
    {
        // compute the distance map based on player's index and all tiles
        // currently all tilesPosition is const
        public static void ComputeDistanceMap(Vector2Int playerIndex, Dictionary<Key, List<Client>> cells)
        {
            // set distance of all walkable tiles to infinity
            foreach (var clientList in cells.Values)
            {
                foreach (var client in clientList)
                {
                    if (client.WalkableTile)
                    {
                        client.DistanceToPlayer = int.MaxValue;
                    }
                }
            }

            // set distance to player field of player client = 0
            Key playerKey = new Key(playerIndex.x, playerIndex.y);
            foreach (var playerCell in cells[playerKey])
            {
                playerCell.DistanceToPlayer = 0;
            }

            // BFS
            Queue<Vector2Int> searchQueue = new Queue<Vector2Int>();
            
            searchQueue.Enqueue(playerIndex);

            while (searchQueue.Count > 0)
            {
                Vector2Int currentCellIndex = searchQueue.Dequeue();
                
                Key currentCellKey = new Key(currentCellIndex.x, currentCellIndex.y);

                // distance of current cell to player
                int currentDistance = cells[currentCellKey][0].DistanceToPlayer;

                foreach (var neighborIndex in GetNeighbors(currentCellIndex, cells))
                {
                    Key k = new Key(neighborIndex.x, neighborIndex.y);
                    
                    foreach(var client in cells[k])
                    {
                        // only process walkable tiles
                        if (client.WalkableTile && client.DistanceToPlayer == int.MaxValue)
                        {
                            client.DistanceToPlayer = currentDistance + 1;
                            searchQueue.Enqueue(neighborIndex);
                        }
                    }
                }
            }
        }

        private static List<Vector2Int> GetNeighbors(Vector2Int element, Dictionary<Key, List<Client>> cells)
        {
            Vector2Int[] candidates = {new Vector2Int(element.x + 1, element.y), new Vector2Int(element.x - 1, element.y), new Vector2Int(element.x, element.y + 1), new Vector2Int(element.x, element.y - 1)};

            List<Vector2Int> myNeighbors = new List<Vector2Int>();

            // add valid neighbor to list
            // LAN_TODO what about the tiles adjacent to player's tile
            for (int i = 0; i < candidates.Length; i++)
            {
                Key k = new Key(candidates[i].x , candidates[i].y);
                if (cells.ContainsKey(k))
                {
                    myNeighbors.Add(candidates[i]);
                }
            }

            return myNeighbors;
        }
    }
}