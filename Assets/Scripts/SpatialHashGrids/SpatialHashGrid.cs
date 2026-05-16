using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpatialHashGrid
{
    private Vector2Int[] bounds; // bounds of the world, in 2D: [[-1000, -1000], [1000, 1000]]
    private Vector2Int dimensions; // dimensions of grids, in 2D [100, 100]
    private Dictionary<Key, List<GridClient>> gridClients; // where the information regarding tile clients and keys are stored
    private const float cellEpsilon = 0.001f;
    private int queryID = 0;

    public Dictionary<Key, List<GridClient>> GetTileCells()
    {
        return gridClients;
    }

    public SpatialHashGrid(Vector2Int[] bounds, Vector2Int dimensions)
    {
        this.bounds = bounds;
        this.dimensions = dimensions;
        gridClients = new Dictionary<Key, List<GridClient>>();
    }

    //---------------------------PUBLIC METHODS-----------------------------
    public GridClient NewGridClient(Vector2 position, Vector2 size)
    {
        GridClient GridClient = new GridClient()
        {
            Position = position,
            Size = size,
            DistanceToPlayer = int.MaxValue,
        };

        InsertGridClient(GridClient);

        return GridClient;
    }

    //---------------------------PRIVATE METHODS-----------------------------
    // insert tile client to grid
    private void InsertGridClient(GridClient gridClient)
    {
        Vector2 position = gridClient.Position;
        Vector2 clientSize = gridClient.Size;

        // index is the bottom-left and top-right int position of a cell 
        Vector2Int i1 = GetCellIndex(new Vector2(position.x - clientSize.x / 2, position.y - clientSize.y / 2));
        Vector2Int i2 = GetCellIndex(new Vector2(position.x + clientSize.x / 2 - cellEpsilon, position.y + clientSize.y / 2 - cellEpsilon));

        gridClient.Bounds = new Vector2Int[] { i1, i2 };

        // construct key for the cell-map
        for (int x = i1.x; x <= i2.x; x++)
        {
            for (int y = i1.y; y <= i2.y; y++)
            {
                Key k = new Key(x, y);

                // if a brand new key then create a list associate with it
                if (!gridClients.ContainsKey(k))
                {
                    gridClients[k] = new List<GridClient>();
                }

                gridClients[k].Add(gridClient);
            }
        }
    }

    //
    private void DeleteGridClient(GridClient gridClient)
    {
        Vector2Int i1 = gridClient.Bounds[0];
        Vector2Int i2 = gridClient.Bounds[1];

        for (int x = i1.x; x <= i2.x; x++)
        {
            for (int y = i1.y; y<= i2.y; y++)
            {
                Key k = new Key(x, y);

                if (gridClients.ContainsKey(k))
                {
                    gridClients[k].Remove(gridClient);  
                }
            }
        }
    }

    /// <summary>
    /// Get index of a cell based on its position
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    private Vector2Int GetCellIndex(Vector2 position)
    {
        Vector2Int cellSize = new Vector2Int(
            (bounds[1][0] - bounds[0][0]) / dimensions[0],
            (bounds[1][1] - bounds[0][1]) / dimensions[1]
        );

        int xIndex = Mathf.Clamp((int)Math.Floor((position.x - bounds[0][0]) / cellSize.x), 0, dimensions[0] - 1);
        int yIndex = Mathf.Clamp((int)Math.Floor((position.y - bounds[0][1]) / cellSize.y), 0, dimensions[1] - 1);

        return new Vector2Int(xIndex, yIndex);
    }
}
