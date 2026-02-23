using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class SquareClient : MonoBehaviour
{
    [SerializeField] private Demo demo;
    private Client client;
    private HashSet<Client> myNearbyClients = new HashSet<Client>();

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
            return transform.localScale;
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

    // Start is called before the first frame update
    void Start()
    {
        client = demo.grid.NewClient(Position, Dimensions, Name);
        Debug.Log($"this {gameObject} is located from index {client.Indices[0]} to {client.Indices[1]}");
    }

    // Update is called once per frame
    void Update()
    {
        //myNearbyClients = demo.grid.FindNear(Position, area);
        //demo.grid.UpdateGrid(client);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        Gizmos.DrawWireCube(Position, area);
    }
}
