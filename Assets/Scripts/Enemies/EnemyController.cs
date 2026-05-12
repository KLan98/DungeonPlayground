using PathFinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class EnemyController : MonoBehaviour, IObserver
{
    [SerializeField] private DungeonGrid dungeonGrid;
    [SerializeField] private EntityType entityType; // choose entity type in inspector

    private Client client;
    [SerializeField] private List<Client> nearByClients = new List<Client>();
    private ChasePlayerTree chasePlayerTree;

    private Vector2 position
    {
        get { return this.gameObject.transform.position; }
    }

    private Vector2 dimension = new Vector2(0.9f, 0.9f);


    //-------------------------------PRIVATE METHODS-------------------------------
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
            {
                nearByClients.Add(c);
            }
        }
    }

    private void UpdateGrid()
    {
        UpdateClientInfo();
        dungeonGrid.spatialHashGrid.UpdateGrid(client);
    }

    //--------------------------BUILT-IN METHODS--------------------------------    
    void Start()
    {
        EntitiesManager entitiesManager = EntitiesManager.GetInstance();
        EPhysicsManager physicsManager = EPhysicsManager.GetInstance();

        // ref to statsTable
        entitiesManager.AssignStats(entityType, this.gameObject);

        // unique and will never be change
        byte entityID = entitiesManager.GetEntityID(this.gameObject);

        //EPhysicsManager.GetInstance().AssignPhysics(this.gameObject, entityID);

        physicsManager.AddObserver(this);

        client = dungeonGrid.spatialHashGrid.NewClient(position, dimension, false, entityID);
        client.GameObject = this.gameObject;

        // behavior tree construction
        chasePlayerTree = new ChasePlayerTree(client);

        // assign init position
        transform.position = MyAPI.GetCellCenter(position);
        FindNearbyClients();
        UpdateGrid();
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.yellow;

    //    Gizmos.DrawWireCube(position, dimension);
    //}

    //---------------------------PUBLIC METHODS------------------------------------------------
    public void OnNotify()
    {
        FindNearbyClients();
        UpdateGrid();
        chasePlayerTree.ProcessTree();
    }
}
