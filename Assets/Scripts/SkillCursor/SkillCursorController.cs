using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillCursorController : MonoBehaviour, IToggleGameObject
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    
    [SerializeField] private DungeonGrid dungeonGrid;
    
    [SerializeField] List<Client> nearByClients = new List<Client>();

    [SerializeField] private List<GameObject> targets = new List<GameObject>();

    public List<Client> NearByClients
    {
        get
        {
            return nearByClients;
        }
    }

    [SerializeField] private float moveSpeed = 5f;
    
    [SerializeField] private SkillCursorState cursorState;
    
    private int tileSize = 1;

    private bool positionRequired = false;

    private GameObject target;
    
    public GameObject Target
    {
        get { return target; }
        private set { target = value; }
    }
    
    private Vector2 cursorPosition
    {
        get
        {
            return transform.position;
        }
    }

    private Vector2 confirmedPosition;

    public bool IsActive
    {
        get
        { return gameObject.activeSelf; }
        set
        { gameObject.SetActive(value); }
    }

    private CursorInputActions cursorInput;
    
    private Vector2 direction;
    
    private Vector2 targetPosition;
    
    private bool isMoving = false;
    
    private float timeBetweenMovement = 0.2f;
    
    private int numberOfTargets = 0;

    private Client client;
    private Vector2 position
    {
        get { return gameObject.transform.position; }
    }

    private Vector2 dimension = new Vector2(0.9f, 0.9f);

    private void Awake()
    {
        direction = Vector2.zero;
        cursorInput = new CursorInputActions();
        cursorInput.Enable();
    }
    
    private void Start()
    {
        client = dungeonGrid.spatialHashGrid.NewClient(position, dimension, "SkillCursor");
    }

    private void Update()
    {
        FindNearbyClients();
        UpdateGrid();

        if (cursorInput.CursorActions.Confirm.WasPressedThisFrame())
        {
            target = GetTarget();

            // if target is game object and not in targets list
            if (target != null && !targets.Contains(target))
            {
                // if not enough targets added
                if (targets.Count < numberOfTargets)
                {
                    targets.Add(target);
                }
            }

            // if target is not game object
            else if (target == null)
            {
                if (positionRequired)
                {
                    confirmedPosition = cursorPosition;

                    if (targets.Count == 1)
                    {
                        PlayerSkillHandler.OnTargetsConfirmed(targets, confirmedPosition);
                    }
                }

                else
                {
                    Debug.Log("Player is selecting empty tile");
                    return;
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
                targets.RemoveAt(targets.Count - 1);
            }
        }
    }

    private void FixedUpdate()
    {
        // polling
        if (cursorState.allowCursor == false)
        {
            return;
        }

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

    private IEnumerator MoveToTarget(Vector2 targetPosition)
    {
        isMoving = true;
        target = null; // reset target
        while (Vector2.Distance(cursorPosition, targetPosition) > 0.01f)
        {
            transform.position = Vector2.MoveTowards(
                cursorPosition,
                targetPosition,
                moveSpeed * tileSize * Time.deltaTime
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

    private GameObject GetTarget()
    {
        if (nearByClients.Count == 0)
        {
            return null;
        }

        GameObject target = nearByClients[0].GameObject; // get the first game object 

        if (target == null)
        {
            Debug.Log("Please choose a valid target");
        }

        else
        {
            Debug.Log($"Chosen game object = {target.name}");
        }

        return target;
    }

    public void ToggleGameObjectActive()
    {
        IsActive = !IsActive;

        target = null;
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawWireCube(position, dimension);
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

    public void BeginSelection(SkillRequirements requirements)
    {
        numberOfTargets = requirements.TargetCount;
        Debug.Log($"The requirements for this skill {requirements.SkillID} are: target count: {requirements.TargetCount}, need position: {requirements.NeedPosition} and target type:{requirements.SkillTargetType}");

        if (requirements.NeedPosition)
        {
            positionRequired = true;
        }

        else
        {
            positionRequired = false;
        }

        IsActive = true;
    }
}