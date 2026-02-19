using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareClient : MonoBehaviour
{
    [SerializeField] private Demo demo;
    [SerializeField] private BoxCollider2D collider2D;
    private Client client;
    public HashSet<Client> myNearbyClients = new HashSet<Client>();

    private Vector2 position
    {
        get
        {
            return transform.position; 
        }
    }

    private Vector2 dimensions
    {
        get
        {
            return collider2D.size;
        }
    }

    private Vector2 area = new Vector2(5, 5);

    // Start is called before the first frame update
    void Start()
    {
        client = demo.grid.NewClient(position, dimensions);
    }

    // Update is called once per frame
    void Update()
    {
        myNearbyClients = demo.grid.FindNear(position, area);
    }
}
