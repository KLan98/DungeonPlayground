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

    private void UpdateClientInfo()
    {
        client.Position = position;
    }

    // update the distance map, LAN_TODO this update can only be triggered whenever the player moved
    private void UpdateDistanceMap()
    {
        BFSPathFinding.ComputeDistanceMap(client.Indices[0], dungeonGrid.spatialHashGrid.Cells);
        VisualizeDistanceMap(dungeonGrid.spatialHashGrid.Cells, 15);
    }

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

    private void Update()
    {
        // polling, LAN_TODO fix polling
        FindNearbyClients();
        UpdateGrid();
        UpdateDistanceMap();
    }
}
