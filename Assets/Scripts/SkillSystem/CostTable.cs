using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct CostTable
{
    public byte Level;
    public byte Cost;

    public CostTable(byte level, byte cost)
    {
        Level = level;
        Cost = cost;
    }
}