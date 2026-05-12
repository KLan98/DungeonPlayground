using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathFinding;

public class PlayerController : MonoBehaviour, ISubject
{
    [SerializeField] private DungeonGrid dungeonGrid;
    [SerializeField] private List<Client> nearByClients = new List<Client>();
    [SerializeField] private Client client;
    [SerializeField] private EntityType entityType;
    
    private bool dijkstraMapOn = false;

    private Vector2 position
    {
        get { return this.gameObject.transform.position; }
    }

    private IObserver playerMoveObserver;

    private Vector2 dimension = new Vector2(0.9f, 0.9f);


    //--------------------------EVENT RESPONSE--------------------------------
    public void OnToggleDijkstraMap()
    {
        ToggleColor(dungeonGrid.spatialHashGrid.Cells);
    }

    public void OnPlayerMove()
    {
        FindNearbyClients();
        UpdateGrid();
        UpdateDistanceMap();
        Notify();
    }

    //--------------------------PRIVATE METHODS--------------------------------
    private void ToggleColor(Dictionary<Key, List<Client>> cells)
    {
        if (!dijkstraMapOn)
        {
            foreach (var clientList in cells.Values)
            {
                foreach (var client in clientList)
                {
                    if (client.WalkableTile && client.DistanceToPlayer != int.MaxValue)
                    {
                        TilemapVisualizer.GetInstance().ColorTileByDistance(client.Position, client.DistanceToPlayer);
                    }
                }
            }

            dijkstraMapOn = true;
        }

        else
        {
            foreach (var clientList in cells.Values)
            {
                foreach (var client in clientList)
                {
                    if (client.WalkableTile && client.DistanceToPlayer != int.MaxValue)
                    {
                        TilemapVisualizer.GetInstance().ResetMapColor(client.Position);
                    }
                }
            }

            dijkstraMapOn = false;
        }
    }

    private void UpdateClientInfo()
    {
        client.Position = position;
    }

    private void FindNearbyClients()
    {
        nearByClients.Clear();
        foreach (var c in dungeonGrid.spatialHashGrid.FindNear(position, dimension))
        {
            if (c.ClientID != client.ClientID)
                nearByClients.Add(c);
        }
    }

    private void UpdateGrid()
    {
        UpdateClientInfo();
        dungeonGrid.spatialHashGrid.UpdateGrid(client);
    }

    private void UpdateDistanceMap()
    {
        BFSPathFinding.ComputeDistanceMap(client.Indices[0], dungeonGrid.spatialHashGrid.Cells);
    }

    //--------------------------BUILT-IN METHODS--------------------------------
    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.yellow;

    //    Gizmos.DrawWireCube(position, dimension);
    //}

    void Start()
    {
        EPhysicsManager physicsManager = EPhysicsManager.GetInstance();
        EntitiesManager entitiesManager = EntitiesManager.GetInstance();

        entitiesManager.AssignStats(entityType, this.gameObject);

        byte entityID = entitiesManager.GetEntityID(this.gameObject);

        client = dungeonGrid.spatialHashGrid.NewClient(position, dimension, false, entityID);
        client.GameObject = this.gameObject;

        transform.position = MyAPI.GetCellCenter(position);

        FindNearbyClients();
        UpdateGrid();

        AddObserver(physicsManager);
    }

    //-----------------------PUBLIC METHODS---------------------------------------
    public void AddObserver(IObserver observer)
    {
        playerMoveObserver = observer;
    }

    public void RemoveObserver(IObserver observer)
    {
        
    }

    public void Notify()
    {
        playerMoveObserver.OnNotify();
    }
}