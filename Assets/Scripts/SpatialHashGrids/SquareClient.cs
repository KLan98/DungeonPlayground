using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class SquareClient : MonoBehaviour
{
    [SerializeField] private DungeonGrid dungeonGrid;
    private Client client;
    private List<Client> myNearbyClients = new List<Client>();
    private SpriteRenderer spriteRenderer;
    [SerializeField] private List<Client> clients = new List<Client>();

    private Vector2 Position
    {
        get
        {
            return transform.position; 
        }
    }

    private Vector2 Dimensions
    {
        get
        {
            return spriteRenderer.size;
        }
    }

    private string Name
    {
        get
        {
            return this.gameObject.name;
        }
    }

    private Vector2 area = new Vector2(5, 5);

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        client = dungeonGrid.spatialHashGrid.NewClient(Position, Dimensions, Name, false);
        Debug.Log($"this {gameObject} is located from index {client.Indices[0]} to {client.Indices[1]}");
        Debug.Log($"{this} dimension is {Dimensions}");
        InvokeRepeating(nameof(FindNearbyClients), 0.1f, 0.3f);
        InvokeRepeating(nameof(UpdateGrid), 0.15f, 0.4f);
        InvokeRepeating(nameof(UpdateClientsList), 0.2f, 0.5f);
    }

    private void FindNearbyClients()
    {
        myNearbyClients = dungeonGrid.spatialHashGrid.FindNear(Position, area);
    }

    private void UpdateGrid()
    {
        dungeonGrid.spatialHashGrid.UpdateGrid(client);
    }

    private void UpdateClientsList()
    {
        clients.Clear();
        clients.AddRange(myNearbyClients);
    }
}
