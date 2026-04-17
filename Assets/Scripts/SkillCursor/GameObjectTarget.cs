using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectTarget : ITargetable
{
    public Vector2Int CellIndex { get; }

    private GameObject gameOject;
    public GameObject GameObject 
    { 
        get
        {
            return gameOject;
        }
    }
    
    public GameObjectTarget(GameObject target, Vector2Int cellIndex)
    {
        CellIndex = cellIndex;
        this.gameOject = target;
    }
}
