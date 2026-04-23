using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.Mathematics;
using UnityEngine;

public class SpatialHashGridsOptimized
{
    private Vector2[] bounds; // bounds of the world, in 2D: [[-1000, -1000], [1000, 1000]]
    private Vector2 dimensions; // dimensions of grids, in 2D [100, 100]
    private Dictionary<Key, List<Client>> cells; // where the information regarding clients and keys are stored
    private int queryID;
    private int clientID;
    private const float cellEpsilon = 0.001f;

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
        this.clientID = 0;
    }

    public Client NewClient(Vector2 position, Vector2 dimensions, string name, bool walkableTile)
    {
        // object initializer
        Client client = new Client()
        {
            Name = name,
            Position = position,
            Dimensions = dimensions,
            Indices = null,
            QueryID = -1, // safe value since it is different from default queryID = 0, QueryID of client will later be assigned
            ClientID = clientID++,
            WalkableTile = walkableTile
        };

        Insert(client);

        return client;
    }

    // insert client into grid
    private void Insert(Client client)
    {
        Vector2 pos = client.Position;
        Vector2 size = client.Dimensions;

        Vector2Int i1 = GetCellIndex(new Vector2(pos.x - size.x / 2, pos.y - size.y / 2));
        Vector2Int i2 = GetCellIndex(new Vector2(pos.x + size.x / 2 - cellEpsilon, pos.y + size.y / 2 - cellEpsilon));

        // Remember which cells this client occupies
        client.Indices = new Vector2Int[] { i1, i2 };

        // construct key for the cell-map
        for (int x = i1.x; x <= i2.x; x++)
        {
            for (int y = i1.y; y <= i2.y; y++)
            {
                Key k = new Key(x, y);

                // if a brand new key then create a list associate with it
                if (!cells.ContainsKey(k))
                {
                    cells[k] = new List<Client>();
                }

                cells[k].Add(client);
                //Debug.Log($"{client.Name} inserted into grid with game object position = {client.Position}");
            }
        }
    }

    // position is the where query for nearby clients taking place, area is the search area
    public List<Client> FindNear(Vector2 pos, Vector2 area)
    {
        Vector2Int i1 = GetCellIndex(new Vector2(pos.x - area.x / 2, pos.y - area.y / 2));
        Vector2Int i2 = GetCellIndex(new Vector2(pos.x + area.x / 2 - cellEpsilon, pos.y + area.y / 2 - cellEpsilon));

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
        Vector2 cellSize = new Vector2(
            (bounds[1][0] - bounds[0][0]) / dimensions[0],
            (bounds[1][1] - bounds[0][1]) / dimensions[1]
        );

        int xIndex = Mathf.Clamp((int)Math.Floor((position.x - bounds[0][0]) / cellSize.x), 0, (int)dimensions[0] - 1);
        int yIndex = Mathf.Clamp((int)Math.Floor((position.y - bounds[0][1]) / cellSize.y), 0, (int)dimensions[1] - 1);

        return new Vector2Int(xIndex, yIndex);
    }

    // if client position changed then grid should update its key
    public void UpdateGrid(Client client)
    {
        Vector2 position = client.Position;
        Vector2 dimensions = client.Dimensions;

        Vector2Int i1 = GetCellIndex(new Vector2(position.x - dimensions.x / 2, position.y - dimensions.y / 2));
        Vector2Int i2 = GetCellIndex(new Vector2(position.x + dimensions.x / 2 - cellEpsilon, position.y + dimensions.y / 2 - cellEpsilon));

        Vector2Int storedBottomLeft = client.Indices[0];
        Vector2Int storedTopRight = client.Indices[1];

        if (i1 == storedBottomLeft && i2 == storedTopRight)
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