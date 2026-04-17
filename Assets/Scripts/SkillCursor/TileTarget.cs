using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileTarget : ITargetable
{
    private Vector2Int cellIndex;
    public Vector2Int CellIndex 
    { 
        get
        {
            return cellIndex;
        }
    }

    private Vector2 cellPosition;
    public Vector2 CellPosition
    {
        get
        {
            return cellPosition;
        }
    }

    public TileTarget(Vector2Int gridIndex, Vector2 cellPosition)
    {
        this.cellIndex = gridIndex;
        this.cellPosition = cellPosition;
    }
}
