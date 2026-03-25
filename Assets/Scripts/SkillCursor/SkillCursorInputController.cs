using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SkillCursorInputController : MonoBehaviour, IToggleGameObject
{
    private const int tileSize = 1;
    private Vector2 cursorPosition
    {
        get
        {
            return transform.position;
        }
    }

    public bool IsActive
    {
        get
        {  return gameObject.activeSelf; }
        set
        { gameObject.SetActive(value); }
    }

    private CursorInputActions cursorInput;
    private Vector2 direction;
    private Vector2 targetPosition;
    [SerializeField] private SkillCursorState cursorState;
    private bool isMoving = false;
    [SerializeField] private float moveSpeed = 5f;
    private float timeBetweenMovement = 0.2f;

    private void Awake()
    {
        direction = Vector2.zero;
        cursorInput = new CursorInputActions();
        cursorInput.Enable();
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

        if (cursorInput.CursorActions.Confirm.IsPressed())
        {
            cursorState.target = GetTarget();
        }
    }

    private IEnumerator MoveToTarget(Vector2 targetPosition)
    {
        isMoving = true;

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
        GameObject target = null;   
        if (target == null)
        {
            Debug.Log("Please choose a valid target");
        }

        return target;
    }

    public void ToggleGameObjectActive()
    {
        IsActive = !IsActive;
    }
}
