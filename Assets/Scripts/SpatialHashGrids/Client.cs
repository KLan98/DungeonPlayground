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
    [SerializeField] private int queryID;
    public int QueryID
    {
        get{ return queryID;}
        set{ queryID = value;}
    }
    [SerializeField] private int clientID;
    public int ClientID
    {
        get { return clientID;}
        set { clientID = value;}
    }
    [SerializeField] private GameObject gameObject; // this can be null, for distinction between immovable environments (floor tiles) and movable game objects
    public GameObject GameObject
    {
        get { return this.gameObject; }
        set { this.gameObject = value; }
    }

    //---------------DISTANCE MAP FIELDS & PROPERTIES----------------
    [SerializeField] private bool walkableTile; // OBSTACLES ARE NOT WALKABLE, PLAYER IS NOT WALKABLE TILE
    public bool WalkableTile
    {
        get { return walkableTile;}
        set { walkableTile = value;}
    }
    [SerializeField] private int distanceToPlayer;
    public int DistanceToPlayer
    {
        get { return distanceToPlayer;}
        set { distanceToPlayer = value;}
    }
}