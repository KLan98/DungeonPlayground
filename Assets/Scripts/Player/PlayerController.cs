using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathFinding;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private DungeonGrid dungeonGrid;
    [SerializeField] List<Client> nearByClients = new List<Client>();
    [SerializeField] private Client client;
    [SerializeField] private TilemapVisualizer tilemapVisualizer;

    private Vector2 position
    {
        get { return this.gameObject.transform.position; }
    }

    private Vector2 dimension = new Vector2(0.9f, 0.9f);


    //--------------------------EVENT RESPONSE--------------------------------
    public void FindNearbyClients()
    {
        nearByClients.Clear();
        foreach (var c in dungeonGrid.spatialHashGrid.FindNear(position, dimension))
        {
            if (c.ClientID != client.ClientID)
                nearByClients.Add(c);
        }
    }

    public void UpdateGrid()
    {
        UpdateClientInfo();
        dungeonGrid.spatialHashGrid.UpdateGrid(client);
    }

    public void UpdateDistanceMap()
    {
        BFSPathFinding.ComputeDistanceMap(client.Indices[0], dungeonGrid.spatialHashGrid.Cells);
        VisualizeDistanceMap(dungeonGrid.spatialHashGrid.Cells, 15);
    }

    //--------------------------PRIVATE METHODS--------------------------------
    private void VisualizeDistanceMap(Dictionary<Key, List<Client>> cells, int maxDistance)
    {
        foreach (var clientList in cells.Values)
        {
            foreach (var client in clientList)
            {
                if (client.WalkableTile && client.DistanceToPlayer != int.MaxValue)
                {
                    tilemapVisualizer.ColorTileByDistance(client.Position, client.DistanceToPlayer, maxDistance);
                }
            }
        }
    }
    private void UpdateClientInfo()
    {
        client.Position = position;
    }

    //--------------------------BUILT-IN METHODS--------------------------------
    // Start is called before the first frame update
    void Start()
    {
        sprite = gameObject.GetComponentInChildren<SpriteRenderer>();
        client = dungeonGrid.spatialHashGrid.NewClient(position, dimension, "Player", false);
        client.GameObject = this.gameObject;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawWireCube(position, dimension);
    }
}
