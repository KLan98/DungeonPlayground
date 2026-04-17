using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SkillCursorController : MonoBehaviour, IToggleGameObject
{
    [Header("Game events")]
    [SerializeField] private Vector2Channel vector2Channel;
    [SerializeField] private Vector2IntChannel vector2IntChannel;
    [SerializeField] private GameObjectChannel gameObjectChannel;
    [SerializeField] private GameObjectChannel removeCachedChannel;
    [SerializeField] private GameEvent allTargetsConfirmed;

    [Header("Components")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private DungeonGrid dungeonGrid;

    [Header("Stats")]
    [SerializeField] private float movingSpeed = 5f;
    [SerializeField] private Vector2 dimension = new Vector2(0.9f, 0.9f);

    [Header("Debug")]
    [SerializeField] private List<Client> nearByClients = new List<Client>();
    [SerializeField] private List<GameObject> targets = new List<GameObject>();
    [SerializeField] private Client client;

    // ----------------------------------------PRIVATE FIELDS------------------------------------------
    private Vector2 confirmedPosition;
    
    private Vector2 direction;
    
    private Vector2 targetPosition;
    
    private bool isMoving;

    private bool destinationRequired;
    
    private const float timeBetweenMovement = 0.2f;
    
    private int numberOfTargets; // local for required targets of a skill

    private const int tileSize = 1;

    private CursorInputActions cursorInput;
    
    private GameObject target;

    // ----------------------------------------PRIVATE PROPERTIES------------------------------------------
    private Vector2 cursorPosition
    {
        get
        {
            return transform.position;
        }
    }
    private Vector2 position
    {
        get { return gameObject.transform.position; }
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

    //------------------------------BUILT-IN METHODS-----------------------------
    private void Awake()
    {
        cursorInput = new CursorInputActions();
    }
    
    private void Start()
    {
        client = dungeonGrid.spatialHashGrid.NewClient(position, dimension, "SkillCursor", false);
    }

    private void OnEnable()
    {
        direction = Vector2.zero;
        isMoving = false;
        targets.Clear();
        cursorInput.Enable();
    }

    private void OnDisable()
    {
        cursorInput.Disable();
    }

    // LAN_TODO, remove the polling here!
    // find nearby clients and update grid are called whenever there is a cursor move event raised 
    private void Update()
    {
        FindNearbyClients();
        UpdateGrid();

        if (cursorInput.CursorActions.Confirm.WasPressedThisFrame())
        {
            ITargetable cursorTarget = GetCursorTarget();

            // target is game object
            if (cursorTarget is GameObjectTarget target)
            {
                // if not enough targets added
                if (targets.Count < numberOfTargets)
                {
                    if (!targets.Contains(target.GameObject) && target.GameObject != null)
                    {
                        targets.Add(target.GameObject);
                        // cache target in playerSkillHandler    
                        gameObjectChannel.RaiseEvent(target.GameObject);
                    }
                }
            }

            // target is grid index
            else if (cursorTarget is TileTarget tileTarget)
            {
                if (destinationRequired)
                {
                    confirmedPosition = tileTarget.CellPosition;

                    if (targets.Count == 1)
                    {
                        //pass confirmed position to event channel
                        vector2Channel.RaiseEvent(confirmedPosition);
                    }
                }

                else
                {
                    // if target count of the skill SO and targets list = 0 
                    if (numberOfTargets == 0 && targets.Count == 0)
                    {
                        // raise vector2int event
                        vector2IntChannel.RaiseEvent(tileTarget.CellIndex);
                    }

                    else
                    {
                        Debug.Log("Player selected empty tile");
                    }
                }
            }

            else
            {
                return;
            }
        }

        else if (cursorInput.CursorActions.Undo.WasPressedThisFrame())
        {
            if (targets.Count > 0)
            {
                // undo the latest target
                GameObject removedTarget = targets[targets.Count - 1];
                targets.Remove(removedTarget);
                removeCachedChannel.RaiseEvent(removedTarget);
            }
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

        Gizmos.DrawWireCube(position, dimension);
    }

    //-------------------------------COROUTINES--------------------------
    private IEnumerator MoveToTarget(Vector2 targetPosition)
    {
        isMoving = true;
        target = null; // reset target
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
    private ITargetable GetCursorTarget()
    {
        if (nearByClients.Count > 0)
        {
            foreach (var client in nearByClients)
            {
                if (client.GameObject != null && !client.WalkableTile)
                {
                    return new GameObjectTarget(client.GameObject, client.Indices[0]);
                }

                return new TileTarget(client.Indices[0], client.Position);
            }
        }

        // else return 
        Debug.LogWarning("There is no valid target");
        return null;
    }

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

    //-----------------------------PUBLIC METHODS---------------------------------
    public void BeginSelection(SkillRequirements requirements)
    {
        numberOfTargets = requirements.TargetCount;
        Debug.Log($"The requirements for this skill {requirements.SkillID} are: target count: {requirements.TargetCount}, need position: {requirements.NeedDestination} and target type: {requirements.SkillTargetType}");

        if (requirements.NeedDestination)
        {
            destinationRequired = true;
        }

        else
        {
            destinationRequired = false;
        }

        IsActive = true;
    }

    public void ToggleGameObjectActive()
    {
        IsActive = !IsActive;

        target = null;
    }
}