using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

[System.Serializable]
public class Client
{
    [SerializeField] private Vector2 position; // should be in both structs?
    public Vector2 Position
    {
        get { return position; }
        set { position = value; }
    }
    [SerializeField] private Vector2 dimensions; // want to remove this
    public Vector2 Dimensions
    {
        get { return dimensions;}
        set { dimensions = value; }
    }
    [SerializeField] private Vector2Int[] indices; // tileclient
    public Vector2Int[] Indices
    {
        get { return indices;}
        set { indices = value; }
    }
    [SerializeField] private int queryID; // optimization field, probably go to tileclient
    public int QueryID
    {
        get{ return queryID;}
        set{ queryID = value;}
    }
    [SerializeField] private int clientID; // want to remove this
    public int ClientID
    {
        get { return clientID;}
        set { clientID = value;}
    }
    [SerializeField] private GameObject gameObject; // this can be null, for distinction between immovable environments (floor tiles) and movable game objects
    public GameObject GameObject // ? need further investigation
    {
        get { return this.gameObject; }
        set { this.gameObject = value; }
    }
    [SerializeField] private byte entityID; // entityclient
    public byte EntityID
    {
        get { return entityID; }
        set { entityID = value; }
    }

    //---------------DJIKSTRA MAP FIELDS & PROPERTIES----------------
    [SerializeField] private bool walkableTile; // no need
    public bool WalkableTile
    {
        get { return walkableTile;}
        set { walkableTile = value;}
    }
    [SerializeField] private int distanceToPlayer; // tileclient
    public int DistanceToPlayer
    {
        get { return distanceToPlayer;}
        set { distanceToPlayer = value;}
    }
}

// do we need a dedicated struct for tile client?
public struct TileClient
{
    public int QueryID; 
}

public struct EntityClient
{
    public GameObject GameObject;
    public byte EntityID;
}

// everything in the map is a grid client, they can either be entity client or tile client
public struct GridClient
{
    public Vector2Int[] Bounds; // bottom left and top right corners
    public Vector2 Position; // position of tile
    public Vector2 Size;
    public int DistanceToPlayer;
}