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
    private SpriteRenderer spriteRenderer;

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
            return spriteRenderer.size;
        }
    }

    private Vector2 area = new Vector2(5, 5);

    private void Awake()
    {
        inputActions = new PlayerInputActions();
        inputActions.Enable();

        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
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
        InvokeRepeating(nameof(FindNearbyClients), 0.1f, 0.3f);
        InvokeRepeating(nameof(UpdateGrid), 0.15f, 0.4f);
        InvokeRepeating(nameof(UpdateClientsList), 0.2f, 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        // debugging methods
        //clients.Clear();
        //clients.AddRange(myNearbyClients);

        //Debug.Log($"this {client.Name} is currently located from index {client.Indices[0]} to {client.Indices[1]}");
    }

    private void FindNearbyClients()
    {
        myNearbyClients = demo.grid.FindNear(position, area);
    }


    private void UpdateGrid()
    {
        demo.grid.UpdateGrid(client);
    }

    private void UpdateClientsList()
    {
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

        client.Position = position;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawWireCube(position, area);
    }
}