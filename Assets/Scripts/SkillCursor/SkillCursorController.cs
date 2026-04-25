using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
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
    [SerializeField] private static List<Client> nearByClients = new List<Client>();
    [SerializeField] private static Client client;

    // ----------------------------------------PRIVATE FIELDS------------------------------------------
    private Vector2 direction;

    private Vector2 targetPosition;

    private bool isMoving;

    private const float timeBetweenMovement = 0.2f;

    private const int tileSize = 1;

    private CursorInputActions cursorInput;

    // ----------------------------------------PRIVATE PROPERTIES------------------------------------------
    private Vector2 cursorPosition
    {
        get
        {
            return transform.position;
        }
    }

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
        client = dungeonGrid.spatialHashGrid.NewClient(cursorPosition, dimension, "SkillCursor", false);
        this.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        direction = Vector2.zero;
        isMoving = false;
        cursorInput.Enable();
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
            targetPosition = GetCellCenter(cursorPosition) + direction * tileSize;
            StartCoroutine(MoveToTarget(targetPosition));
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawWireCube(cursorPosition, dimension);
    }

    //-------------------------------COROUTINES--------------------------
    private IEnumerator MoveToTarget(Vector2 targetPosition)
    {
        isMoving = true;
        while (Vector2.Distance(cursorPosition, targetPosition) > 0.01f)
        {
            transform.position = Vector2.MoveTowards(
                cursorPosition,
                targetPosition,
                movingSpeed * tileSize * Time.deltaTime
            );
            yield return null;
        }

        transform.position = GetCellCenter(cursorPosition);

        yield return WaitUntilNextMovement(timeBetweenMovement);

        skillCursorMoves.Raise();

        isMoving = false;
    }

    private IEnumerator WaitUntilNextMovement(float time)
    {
        yield return new WaitForSeconds(time);
    }

    // take in a worldPosition and convert to cell
    private Vector2 GetCellCenter(Vector2 worldPosition)
    {
        int xIndex = Mathf.FloorToInt(worldPosition.x / tileSize);
        int yIndex = Mathf.FloorToInt(worldPosition.y / tileSize);
        return new Vector2(
            xIndex * tileSize + tileSize / 2f,
            yIndex * tileSize + tileSize / 2f
        );
    }

    //-----------------------------PRIVATE METHODS-------------------------------

    private void FindNearbyClients()
    {
        nearByClients.Clear();
        foreach (var c in dungeonGrid.spatialHashGrid.FindNear(cursorPosition, dimension))
        {
            if (c.ClientID != client.ClientID)
                nearByClients.Add(c);
        }
    }

    private void UpdateGrid()
    {
        client.Position = cursorPosition;
        dungeonGrid.spatialHashGrid.UpdateGrid(client);
    }

    private void UpdateBlastRadius()
    {
        // move the blast radius whenever the cursor moves
    }

    /// <summary>
    /// Create game object blast radius and attach children game objects base on parameter
    /// </summary>
    /// <param name="value">
    /// Number of cells the blast radius has
    /// </param>
    private void CreateBlastRadiusTiles(Vector2 position)
    {



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
        return client.Position;
    }

    public Vector2Int GetCursorIndex()
    {
        return client.Indices[0];
    }

    public void SpawnBlastRadiusTiles (int blastRadius)
    {
        Vector2 cursorPosition = GetCursorPosition();
        Vector2Int cursorIndex = GetCursorIndex();

        GameObject origin = new GameObject("BlastRadius");
        Material material = Resources.Load<Material>("Materials/BlastRadius");

        for (int x = -blastRadius; x <= blastRadius; x++)
        {
            for (int y = -blastRadius; y <= blastRadius; y++)
            {
                int manhattanDistance = Mathf.Abs(x) + Mathf.Abs(y);

                if (manhattanDistance > 0 && manhattanDistance <= blastRadius)
                {
                    Vector2Int tileIndex = new Vector2Int(x, y);
                    Vector2 tilePosition = cursorPosition + tileIndex;

                    GameObject tile = new GameObject("Tile");
                    tile.AddComponent<MeshRenderer>().material = material;
                    tile.AddComponent<MeshFilter>();
                    tile.AddComponent<VertexMesh>();
                    tile.transform.SetParent(origin.transform);
                    tile.transform.position = tilePosition;
                }
            }
        }

    }

    //------------------------------EVENT RESPONSES------------------------------------------
    public void OnCursorMove()
    {
        FindNearbyClients();
        foreach (var client in nearByClients)
        {
            Debug.Log($"Nearby client {client.Name}");
        }
        Debug.Log("-----------------------");
        UpdateGrid();
        UpdateBlastRadius();
    }

    public void OnToggleActive()
    {
        IsActive = !IsActive;
    }
}