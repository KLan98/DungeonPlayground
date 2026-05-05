using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class SkillCursorController : MonoBehaviour, IToggleGameObject
{
    [Header("Game events")]
    [SerializeField] private Vector2Channel vector2Channel;
    [SerializeField] private Vector2IntChannel vector2IntChannel;
    [SerializeField] private GameObjectChannel gameObjectChannel;
    [SerializeField] private GameObjectChannel removeCachedChannel;
    [SerializeField] private GameEvent allTargetsConfirmed;
    [SerializeField] private GameEvent skillCursorMoves;
    [SerializeField] private GameEvent cursorConfirmed;
    [SerializeField] private GameEvent interruptSkill;

    [Header("Components")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private DungeonGrid dungeonGrid;

    [Header("Stats")]
    [SerializeField] private float movingSpeed = 5f;
    [SerializeField] private Vector2 dimension = new Vector2(0.9f, 0.9f);

    [Header("Debug")]
    [SerializeField] private List<Client> nearByClients = new List<Client>();
    [SerializeField] private Client client;

    // ----------------------------------------PRIVATE FIELDS------------------------------------------
    private Vector2 direction;

    private Vector2 targetPosition;

    private bool isMoving;

    private const float timeBetweenMovement = 0.2f;

    private const int tileSize = 1;

    private CursorInputActions cursorInput;

    // ----------------------------------------PUBLIC PROPERTIES------------------------------------------
    public bool IsActive
    {
        get
        { return gameObject.activeSelf; }
        set
        { gameObject.SetActive(value); }
    }

    public List<Client> NearByClients
    {
        get
        {
            return nearByClients;
        }
    }

    //-----------------------------------------------PUBLIC FIELDS-------------------------
    public static SkillCursorController Instance { get; private set; }

    //------------------------------BUILT-IN METHODS-----------------------------
    private void Awake()
    {
        cursorInput = new CursorInputActions();
        Instance = this;
    }

    private void Start()
    {
        client = dungeonGrid.spatialHashGrid.NewClient(GetCursorPosition(), dimension, "SkillCursor", false);
        this.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        direction = Vector2.zero;
        isMoving = false;
        cursorInput.Enable();

        // snap the position into index
        transform.position = MyAPI.GetCellCenter(GetCursorPosition());
    }

    private void OnDisable()
    {
        cursorInput.Disable();
    }

    private void Update()
    {
        if (cursorInput.CursorActions.Confirm.WasPressedThisFrame())
        {
            cursorConfirmed.Raise();
        }

        else if (cursorInput.CursorActions.Interrupt.WasPressedThisFrame())
        {
            interruptSkill.Raise();
        }
    }

    private void FixedUpdate()
    {
        if (isMoving)
        {
            return;
        }

        if (cursorInput.CursorActions.Up.IsPressed())
        {
            direction = Vector2.up;
        }

        else if (cursorInput.CursorActions.Down.IsPressed())
        {
            direction = Vector2.down;
        }

        else if (cursorInput.CursorActions.Left.IsPressed())
        {
            direction = Vector2.left;
        }

        else if (cursorInput.CursorActions.Right.IsPressed())
        {
            direction = Vector2.right;
        }

        else
        {
            direction = Vector2.zero;
        }

        if (direction != Vector2.zero)
        {
            targetPosition = MyAPI.GetCellCenter(GetCursorPosition()) + direction;
            StartCoroutine(MoveToTarget(targetPosition));
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawWireCube(GetCursorPosition(), dimension);
    }

    //-------------------------------COROUTINES--------------------------
    private IEnumerator MoveToTarget(Vector2 targetPosition)
    {
        isMoving = true;
        while (Vector2.Distance(GetCursorPosition(), targetPosition) > 0.01f)
        {
            transform.position = Vector2.MoveTowards(
                GetCursorPosition(),
                targetPosition,
                movingSpeed * tileSize * Time.deltaTime
            );
            yield return null;
        }

        transform.position = MyAPI.GetCellCenter(GetCursorPosition());

        yield return WaitUntilNextMovement(timeBetweenMovement);

        skillCursorMoves.Raise();

        isMoving = false;
    }

    private IEnumerator WaitUntilNextMovement(float time)
    {
        yield return new WaitForSeconds(time);
    }

    //-----------------------------PRIVATE METHODS-------------------------------

    private void FindNearbyClients()
    {
        nearByClients.Clear();
        foreach (var c in dungeonGrid.spatialHashGrid.FindNear(GetCursorPosition(), dimension))
        {
            if (c.ClientID != client.ClientID)
                nearByClients.Add(c);
        }
    }

    private void UpdateGrid()
    {
        client.Position = GetCursorPosition();
        dungeonGrid.spatialHashGrid.UpdateGrid(client);
    }

    //-----------------------------PUBLIC METHODS---------------------------------
    public ITargetable GetCursorTarget()
    {
        if (nearByClients.Count > 0)
        {
            foreach (var client in nearByClients)
            {
                if (client.GameObject != null && !client.WalkableTile)
                {
                    return new GameObjectTarget(client.GameObject, client.Indices[0]);
                }
            }

            // if no game object target found then return tile target with cursor's parameters
            return new TileTarget(client.Indices[0], client.Position);
        }

        // else return 
        Debug.LogWarning("There is no valid target");
        return null;
    }

    public Vector2 GetCursorPosition()
    {
        return transform.position;
    }

    public Vector2Int GetCursorIndex()
    {
        return client.Indices[0];
    }

    public GameObject SpawnBlastRadiusTiles (byte blastRadius)
    {
        Vector2Int cursorIndex = GetCursorIndex();

        GameObject origin = new GameObject("BlastRadius");
        origin.transform.SetParent(this.transform);
        origin.transform.position = new Vector3(transform.position.x, transform.position.y);
        Material material = Resources.Load<Material>("Materials/BlastRadius");

        for (int x = -blastRadius; x <= blastRadius; x++)
        {
            for (int y = -blastRadius; y <= blastRadius; y++)
            {
                int manhattanDistance = Mathf.Abs(x) + Mathf.Abs(y);

                if (manhattanDistance >= 0 && manhattanDistance <= blastRadius)
                {
                    Vector2Int tileIndex = new Vector2Int(x, y);
                    GameObject tile = new GameObject("Tile");
                    tile.AddComponent<MeshRenderer>().material = material;
                    tile.AddComponent<MeshFilter>();
                    tile.AddComponent<VertexMesh>();
                    tile.transform.SetParent(origin.transform);
                    tile.transform.position = new Vector3(origin.transform.position.x + tileIndex.x - 0.5f, origin.transform.position.y + tileIndex.y - 0.5f, origin.transform.position.z); 
                }
            }
        }

        return origin;
    }
    
    public void ToggleActive()
    {
        IsActive = !IsActive;
    }

    public void DestroyBlastRadius(GameObject blastRadius)
    {
        Destroy(blastRadius);
    }

    //------------------------------EVENT RESPONSES------------------------------------------
    public void OnCursorMove()
    {
        FindNearbyClients();
        UpdateGrid();
        foreach (var client in nearByClients)
        {
            Debug.Log($"Nearby client {client.Name}");
        }
        Debug.Log("-----------------------");
    }
}