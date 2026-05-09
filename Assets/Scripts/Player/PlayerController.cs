using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathFinding;

public class PlayerController : MonoBehaviour
{
    //[SerializeField] private SpriteRenderer sprite;
    [SerializeField] private DungeonGrid dungeonGrid;
    [SerializeField] List<Client> nearByClients = new List<Client>();
    [SerializeField] private Client client;
    [SerializeField] private TilemapVisualizer tilemapVisualizer;
    private bool dijkstraMapOn = false;

    private EntityType entityType;
    private Entity entity;

    private Vector2 position
    {
        get { return this.gameObject.transform.position; }
    }

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
                        tilemapVisualizer.ColorTileByDistance(client.Position, client.DistanceToPlayer);
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
                        tilemapVisualizer.ResetMapColor(client.Position);
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
    private void Awake()
    {
        entityType = EntityType.PLAYER;
    }

    // Start is called before the first frame update
    void Start()
    {
        //sprite = gameObject.GetComponentInChildren<SpriteRenderer>();
        client = dungeonGrid.spatialHashGrid.NewClient(position, dimension, "Player", false);
        client.GameObject = this.gameObject;

        //EntitiesManager.GetInstance().GetEntitiesDatabase().AddAliveEntity(client);

        //EntitiesManager.GetInstance().AddRoomEntity(this.gameObject);
        EntitiesManager.GetInstance().AssignStats(entityType, this.gameObject);

        transform.position = MyAPI.GetCellCenter(position);
        OnPlayerMove();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawWireCube(position, dimension);
    }
}
