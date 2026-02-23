using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

[System.Serializable]
public class Client
{
    [SerializeField] private string name;
    public string Name
    {
        get { return name; }
        set { name = value; }   
    }
    [SerializeField] private Vector2 position;
    public Vector2 Position
    {
        get { return position; }
        set { position = value; }
    }
    [SerializeField] private Vector2 dimensions;
    public Vector2 Dimensions
    {
        get { return dimensions;}
        set { dimensions = value; }
    }
    [SerializeField] private Vector2Int[] indices;
    public Vector2Int[] Indices
    {
        get { return indices;}
        set { indices = value; }
    }
}