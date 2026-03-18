using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

public class SpatialHashGridsOptimized
{
    private Vector2[] bounds; // bounds of the world, in 2D: [[-1000, -1000], [1000, 1000]]
    private Vector2 dimensions; // dimensions of grids, in 2D [100, 100]
    private Dictionary<Key, List<Client>> cells; // where the information regarding clients and keys are stored
    private int queryID;

    public Dictionary<Key, List<Client>> Cells
    {
        get
        {
            return cells;
        }
    }

    public SpatialHashGridsOptimized(Vector2[] bounds, Vector2 dimensions)
    {
        this.bounds = bounds;
        this.dimensions = dimensions;
        cells = new Dictionary<Key, List<Client>>();
        this.queryID = 0;
    }

    public Client NewClient(Vector2 position, Vector2 dimensions, string name)
    {
        // object initializer
        Client client = new Client()
        {
            Name = name,
            Position = position,
            Dimensions = dimensions,
            Indices = null,
            QueryID = -1 // safe value since it is different from default queryID = 0, QueryID of client will later be assigned
        };

        Insert(client);

        return client;
    }

    // insert client into grid
    private void Insert(Client client)
    {
        Vector2 pos = client.Position;
        Vector2 size = client.Dimensions;

        Vector2Int i1 = GetCellIndex(new Vector2(pos.x - size.x / 2, pos.y - size.y / 2)); // bottom left of cells in the grid that contain the client
        Vector2Int i2 = GetCellIndex(new Vector2(pos.x + size.x / 2, pos.y + size.y / 2)); // top right of cells in the grid that contain the client

        // Remember which cells this client occupies
        client.Indices = new Vector2Int[] { i1, i2 };

        // construct key for the cell-map
        for (int x = i1.x; x <= i2.x; x++)
        {
            for (int y = i1.y; y <= i2.y; y++)
            {
                Key k = new Key(x, y);

                if (!this.cells.ContainsKey(k))
                {
                    // need this since in the first interation the key does not exist
                    cells[k] = new List<Client>();
                }

                cells[k].Add(client);
            }
        }
    }

    // position is the where query for nearby clients taking place, area is the search area
    public List<Client> FindNear(Vector2 pos, Vector2 area)
    {
        Vector2Int i1 = GetCellIndex(new Vector2(pos.x - area.x / 2, pos.y - area.y / 2)); // bottom left of cells in the grid that contain the client
        Vector2Int i2 = GetCellIndex(new Vector2(pos.x + area.x / 2, pos.y + area.y / 2)); // top right of cells in the grid that contain the client

        int currentqueryID = queryID++;

        List<Client> myNearbyClients = new List<Client>();  

        for (int x = i1.x; x <= i2.x; x++)
        {
            for (int y = i1.y; y <= i2.y; y++)
            {
                Key k = new Key(x, y);

                // the output bucket is a list of clients, there maybe client that spans multiple cells -> duplication
                if (cells.TryGetValue(k, out var bucket))
                {
                    // iterate through each client in this bucket, there maybe duplication
                    foreach (var client in bucket)
                    {
                        // if the queryID of this client is different from current query ID means this client has never been seen before -> substitution of deduplicate by hashset -> ensure that every client is added once to neabyClients List
                        if (client.QueryID != currentqueryID)
                        {
                            client.QueryID = currentqueryID;
                            myNearbyClients.Add(client);
                        }
                    }
                }
            }
        }

        return myNearbyClients;
    }

    // compute the x y index of a cell
    private Vector2Int GetCellIndex(Vector2 position)
    {
        // (position - min bound) / (max bound - min bound)
        var x = math.saturate((position.x - this.bounds[0][0]) / (this.bounds[1][0] - this.bounds[0][0]));
        // So if the grid goes from `0` to `100` and the object is at `25`:
        // x = (25 - 0) / (100 - 0) = 0.25  →  25 % across the grid
        var y = math.saturate((position.y - this.bounds[0][1]) / (this.bounds[1][1] - this.bounds[0][1]));

        // base-0 so need to -1 
        // 249.2505 → 249(down)
        // 249.5 → 250(up)
        int xIndex = (int)Math.Round(x * (this.dimensions[0] - 1), 0, MidpointRounding.AwayFromZero);
        int yIndex = (int)Math.Round(y * (this.dimensions[1] - 1), 0, MidpointRounding.AwayFromZero);

        return new Vector2Int(xIndex, yIndex);
    }

    // if client position changed then grid should update its key
    public void UpdateGrid(Client client)
    {
        Vector2 position = client.Position;
        Vector2 dimensions = client.Dimensions;
        Vector2Int[] index = client.Indices;
        Vector2Int storedBottomLeft = index[0];
        Vector2Int storedTopRight = index[1];

        var x1 = position.x - dimensions.x / 2;
        var y1 = position.y - dimensions.y / 2;
        Vector2 bottomLeft = new Vector2(x1, y1);

        var x2 = position.x + dimensions.x / 2;
        var y2 = position.y + dimensions.y / 2;
        Vector2 topRight = new Vector2(x2, y2);

        Vector2Int i1 = GetCellIndex(bottomLeft);
        Vector2Int i2 = GetCellIndex(topRight);

        if (i1.x == storedBottomLeft.x && i1.y == storedBottomLeft.y && i2.x == storedTopRight.x && i2.y == storedTopRight.y)
        {
            return;
        }

        Delete(client);
        Insert(client);
    }

    // delete client from grid
    private void Delete(Client client)
    {
        Vector2Int i1 = client.Indices[0];
        Vector2Int i2 = client.Indices[1];

        // construct key for the cell-map
        for (int x = i1.x; x <= i2.x; x++)
        {
            for (int y = i1.y; y <= i2.y; y++)
            {
                Key k = new Key(x, y);

                if (cells.ContainsKey(k))
                {
                    cells[k].Remove(client);
                }
            }
        }
    }

    public struct Key
    {
        private int x;
        private int y;

        public Key(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }
}