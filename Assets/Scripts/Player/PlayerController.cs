using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private DungeonGrid dungeonGrid;
    [SerializeField] private int myID;
    [SerializeField] List<Client> nearByClients = new List<Client>();
    private Client client;

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

    // Start is called before the first frame update
    void Start()
    {
        sprite = gameObject.GetComponentInChildren<SpriteRenderer>();
        client = dungeonGrid.spatialHashGrid.NewClient(position, dimension, "Player");
        client.GameObject = this.gameObject;
        myID = client.ClientID;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawWireCube(position, dimension);
    }

    private void Update()
    {
        // polling
        FindNearbyClients();
        UpdateGrid();
    }
}
