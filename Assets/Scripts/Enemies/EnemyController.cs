using PathFinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] DungeonGrid dungeonGrid;
    [SerializeField] List<Client> nearByClients = new List<Client>();
    [SerializeField] private Client client;
    [SerializeField] private TilemapVisualizer tilemapVisualizer;
    [SerializeField] private List<Client> myPath = new List<Client>();

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

    public void PathFinding()
    {
        BFSPathFinding.PathFinding(client, dungeonGrid.spatialHashGrid.Cells, myPath);
    }

    private void UpdateClientInfo()
    {
        client.Position = position;
    }

    //--------------------------BUILT-IN METHODS--------------------------------
    // Start is called before the first frame update
    void Start()
    {
        client = dungeonGrid.spatialHashGrid.NewClient(position, dimension, "Actor", false);
        client.GameObject = this.gameObject;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawWireCube(position, dimension);
    }
}
