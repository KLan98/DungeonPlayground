using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSquare : MonoBehaviour
{
    private PlayerInputActions inputActions;
    [SerializeField] private Demo demo;
    [SerializeField] private BoxCollider2D collider2D;
    [SerializeField] private Rigidbody2D rb;

    private Client client;
    private HashSet<Client> myNearbyClients = new HashSet<Client>();

    public float speed = 5f;
    private Vector2 moveInput = Vector2.zero;

    [SerializeField] private List<Client> clients = new List<Client>();

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

    private void Awake()
    {
        inputActions = new PlayerInputActions();
        inputActions.Enable();

        rb = GetComponent<Rigidbody2D>();
    }

    private string Name
    {
        get
        {
            return this.gameObject.name;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        client = demo.grid.NewClient(position, dimensions, Name);
        Debug.Log($"this {gameObject} is located from index {client.Indices[0]} to {client.Indices[1]}");
    }

    // Update is called once per frame
    void Update()
    {
        myNearbyClients = demo.grid.FindNear(position, area);
        clients.Clear();
        clients.AddRange(myNearbyClients);
    }

    private void FixedUpdate()
    {
        moveInput = Vector2.zero;

        if (inputActions.Movement.Down.IsPressed())
        {
            moveInput += Vector2.down;
        }

        if (inputActions.Movement.Left.IsPressed())
        {
            moveInput += Vector2.left;
        }

        if (inputActions.Movement.Right.IsPressed())
        {
            moveInput += Vector2.right;
        }

        if (inputActions.Movement.Up.IsPressed())
        {
            moveInput += Vector2.up;
        }

        rb.velocity = moveInput.normalized * speed;

        // update grid with the latest info of this client
        if (rb.velocity.magnitude > 0)
        {
            demo.grid.UpdateGrid(client);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawWireCube(position, area);
    }
}