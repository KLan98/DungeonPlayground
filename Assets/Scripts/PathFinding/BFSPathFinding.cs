using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PathFinding
{
    public class BFSPathFinding
    {
        // compute the distance map based on player's index and cells map
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

            // set distance to player of player client = 0
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

                int currentDistance = 0;

                // distance of current walkable tile to player
                foreach (var cell in cells[currentCellKey])
                {
                    if (cell.WalkableTile)
                    {
                        currentDistance = cell.DistanceToPlayer;
                    }
                }

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

        //public static void ComputeDijkstraMap(Vector2Int playerIndex, Dictionary<Key, List<GridClient>> gridClients)
        //{
        //    // BFS 
        //    Queue<Vector2Int> searchQueue = new Queue<Vector2Int>();
        //    searchQueue.Enqueue(playerIndex);

        //    // iterate through all cells of the grid by their index 
        //    while (searchQueue.Count > 0)
        //    {
        //        Vector2Int currentIndex = searchQueue.Dequeue();

        //        Key key = new Key(currentIndex.x, currentIndex.y);

        //        int currentDistance = 0;
                
        //        // set the distance for each client
        //        foreach (var client in gridClients[key])
        //        {
        //            currentDistance = client.DistanceToPlayer;
        //        }

        //        foreach (var neighborIndex in GetTileNeighbors(currentIndex, gridClients))
        //        {
        //            Key k = new Key(neighborIndex.x, neighborIndex.y);

        //            foreach (var client in gridClients[k])
        //            {
        //                int distanceToPlayer = client.DistanceToPlayer;
        //                distanceToPlayer = currentDistance + 1;
        //                client.DistanceToPlayer = distanceToPlayer;
        //            }
        //        }
        //    }
        //}

        public static List<Client> PathFinding(Client actor, Dictionary<Key, List<Client>> cells, List<Client> myPath)
        {
            //Debug.Log("BFS path finding called");
            // clear the output path
            myPath.Clear();

            // assign the distance to player of this actor = distance to player of the tile it is standing in
            Key actorKey = new Key(actor.Indices[0][0], actor.Indices[0][1]);
            if (cells.TryGetValue(actorKey, out List<Client> list))
            {
                if (list.Count == 2)
                {
                    foreach(var client in list)
                    {
                        if (client.DistanceToPlayer != int.MaxValue && client.WalkableTile)
                        {
                            actor.DistanceToPlayer = client.DistanceToPlayer;
                        }
                    }
                }
            }

            // BFS
            // The search queue consists of tile!
            Queue<Client> searchQueue = new Queue<Client>();
            searchQueue.Enqueue(actor);
            
            while(searchQueue.Count > 0)
            {
                Client currentTileCell = searchQueue.Dequeue();

                List<Vector2Int> myNeighbors = GetNeighbors(currentTileCell.Indices[0], cells);

                // iterate through all neighbors
                foreach(var neighbor in myNeighbors)
                {
                    Key k = new Key(neighbor.x, neighbor.y);

                    foreach(var tileCell in cells[k])
                    {
                        if (tileCell.WalkableTile && tileCell.DistanceToPlayer != int.MaxValue && tileCell.DistanceToPlayer < actor.DistanceToPlayer)
                        {
                            // the client only moves to the tile cell with shortest distance to player
                            actor.DistanceToPlayer = tileCell.DistanceToPlayer;

                            //---------------DEBUG---------------------
                            //Debug.Log($"Entity {actor.EntityID} moves to tile with distance {tileCell.DistanceToPlayer} at {tileCell.Position}");
                            myPath.Add(tileCell);
                            searchQueue.Enqueue(tileCell);
                        }
                    }
                }
            }

            //Debug.Log("Return myPath");
            return myPath;
        }

        //public static List<TileClient> EntityPathFinding(EntityClient entity, Dictionary<Key, List<TileClient>> tileCells, List<TileClient> myPath)
        //{
        //    //Debug.Log("BFS path finding called");
        //    // clear the output path
        //    myPath.Clear();

        //    // assign the distance to player of this actor = distance to player of the tile it is standing in
        //    Key entityKey = new Key(entity.Indices[0][0], entity.Indices[0][1]);
        //    if (tileCells.TryGetValue(entityKey, out List<Client> list))
        //    {
        //        if (list.Count == 2)
        //        {
        //            foreach (var client in list)
        //            {
        //                if (client.DistanceToPlayer != int.MaxValue && client.WalkableTile)
        //                {
        //                    entity.DistanceToPlayer = client.DistanceToPlayer;
        //                }
        //            }
        //        }
        //    }

        //    // BFS
        //    // The search queue consists of tile!
        //    Queue<TileClient> searchQueue = new Queue<TileClient>();
        //    searchQueue.Enqueue(entity);

        //    while (searchQueue.Count > 0)
        //    {
        //        Client currentTileCell = searchQueue.Dequeue();

        //        List<Vector2Int> myNeighbors = GetNeighbors(currentTileCell.Indices[0], tileCells);

        //        // iterate through all neighbors
        //        foreach (var neighbor in myNeighbors)
        //        {
        //            Key k = new Key(neighbor.x, neighbor.y);

        //            foreach (var tileCell in tileCells[k])
        //            {
        //                if (tileCell.WalkableTile && tileCell.DistanceToPlayer != int.MaxValue && tileCell.DistanceToPlayer < entity.DistanceToPlayer)
        //                {
        //                    // the client only moves to the tile cell with shortest distance to player
        //                    entity.DistanceToPlayer = tileCell.DistanceToPlayer;

        //                    //---------------DEBUG---------------------
        //                    //Debug.Log($"Entity {actor.EntityID} moves to tile with distance {tileCell.DistanceToPlayer} at {tileCell.Position}");
        //                    myPath.Add(tileCell);
        //                    searchQueue.Enqueue(tileCell);
        //                }
        //            }
        //        }
        //    }

        //    //Debug.Log("Return myPath");
        //    return myPath;
        //}

        private static List<Vector2Int> GetNeighbors(Vector2Int element, Dictionary<Key, List<Client>> cells)
        {
            Vector2Int[] candidates = {new Vector2Int(element.x + 1, element.y), new Vector2Int(element.x - 1, element.y), new Vector2Int(element.x, element.y + 1), new Vector2Int(element.x, element.y - 1)};

            List<Vector2Int> myNeighbors = new List<Vector2Int>();

            // add valid neighbor to list
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

        /// <summary>
        /// Return the list of indices of neighboring tiles
        /// </summary>
        /// <param name="element"></param>
        /// <param name="gridClients"></param>
        /// <returns></returns>
        private static List<Vector2Int> GetTileNeighbors(Vector2Int element, Dictionary<Key, List<GridClient>> gridClients)
        {
            Vector2Int[] candidates = { new Vector2Int(element.x + 1, element.y), new Vector2Int(element.x - 1, element.y), new Vector2Int(element.x, element.y + 1), new Vector2Int(element.x, element.y - 1) };

            List<Vector2Int> myNeighbors = new List<Vector2Int>();

            // add valid neighbor to list
            for (int i = 0; i < candidates.Length; i++)
            {
                Key k = new Key(candidates[i].x, candidates[i].y);
                if (gridClients.ContainsKey(k))
                {
                    myNeighbors.Add(candidates[i]);
                }
            }

            return myNeighbors;
        }
    }
}