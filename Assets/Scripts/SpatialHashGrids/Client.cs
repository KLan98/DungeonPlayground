using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Client
{
    public Vector2 Position; // position of client 
    public Vector2 Dimensions; // dimensions of client
    public Vector2Int[] Indices; // which index is the client in the grid
}