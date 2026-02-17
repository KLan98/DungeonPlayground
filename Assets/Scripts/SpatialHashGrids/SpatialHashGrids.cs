using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

public class SpatialHashGrids 
{
    private Vector2[] bounds; // bounds of the world, in 2D: [[-1000, -1000], [1000, 1000]]
    private Vector2 dimensions; // dimensions of grids, in 2D [100, 100]
    private Dictionary<string, HashSet<Client>> cells; // properties of cells

    public SpatialHashGrids(Vector2[] bounds, Vector2 dimensions)
    {
        this.bounds = bounds;
        this.dimensions = dimensions;
        cells = new Dictionary<string, HashSet<Client>> ();
    }

    public Client NewClient(Vector2 position, Vector2 dimensions)
    {
        // object initializer
        Client client = new Client()
        {
            Position = position,
            Dimensions = dimensions,
            Indices = null
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

        // Remember which cells this object occupies
        client.Indices = new Vector2Int[] { i1, i2 };

        // construct key for the cell-map
        for (int x = i1.x; x <= i2.x; x++)
        {
            for (int y = i1.y; y <= i2.y; y++)
            {
                string k = Key(x, y);

                // if the cells do not have this key then create a new HashSet with it
                if (!this.cells.ContainsKey(k))
                {
                    cells[k] = new HashSet<Client>();
                }

                cells[k].Add(client); // always add client, new or existing cell
            }
        }
    }

    // compute the x y index of a cell 
    public Vector2Int GetCellIndex(Vector2 position)
    {
        // (position - min bound) / (max bound - min bound)
        var x = math.saturate((position.x - this.bounds[0][0]) / (this.bounds[1][0] - this.bounds[0][0]));
        // So if the grid goes from `0` to `100` and the object is at `25`:
        // x = (25 - 0) / (100 - 0) = 0.25  →  25 % across the grid
        var y = math.saturate((position.y - this.bounds[0][1]) / (this.bounds[1][1] - this.bounds[0][1]));

        // base-0 so need to -1 
        int xIndex = (int)math.floor(x * (this.dimensions[0] - 1));
        int yIndex = (int)math.floor(y * (this.dimensions[1] - 1));

        return new Vector2Int(xIndex, yIndex);
    }

    private string Key(int x, int y)
    {
        return x + "." + y;
    }
}