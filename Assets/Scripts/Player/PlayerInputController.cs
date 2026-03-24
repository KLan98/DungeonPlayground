using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// all of the input that the player can do
public class PlayerInputController : MonoBehaviour
{
    private const int tileSize = 1;
    private PlayerInputActions inputActions;
    private Vector2 playerPosition
    {
        get
        {
            return this.transform.position;
        }
    }

    private Vector2 direction;
    private Vector2 targetPosition;
    private bool isMoving = false; // is the MoveToTarget coroutine being run
    private float moveSpeed = 5f; // will be removed
    private float timeBetweenMovement = 0.2f;

    private void Awake()
    {
        direction = Vector2.zero;
        inputActions = new PlayerInputActions();
        inputActions.Enable();
    }

    private void FixedUpdate()
    {
        if (isMoving)
        {
            return;
        }

        // polling
        if (inputActions.PlayerActions.Up.IsPressed())
        {
            direction = Vector2.up;
        }

        else if (inputActions.PlayerActions.Down.IsPressed())
        {
            direction = Vector2.down;
        }
        
        else if (inputActions.PlayerActions.Left.IsPressed())
        {
            direction = Vector2.left;
        }
        
        else if (inputActions.PlayerActions.Right.IsPressed())
        {
            direction = Vector2.right;
        }

        else
        {
            direction = Vector2.zero;
        }
        
        if (direction != Vector2.zero)
        {
            targetPosition = GetCellCenter(playerPosition) + direction * tileSize;
            StartCoroutine(MoveToTarget(targetPosition));
        }
    }

    private IEnumerator MoveToTarget(Vector2 targetPosition)
    {
        isMoving = true;

        while (Vector2.Distance(playerPosition, targetPosition) > 0.01f)
        {
            transform.position = Vector2.MoveTowards(
                playerPosition,
                targetPosition,
                moveSpeed * tileSize * Time.deltaTime
            );
            yield return null;
        }

        transform.position = GetCellCenter(playerPosition);

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

    private void HotBarInputs()
    {
        if (inputActions.PlayerActions.HotBarSlot0.IsPressed())
        {

        }

        else if (inputActions.PlayerActions.HotBarSlot1.IsPressed())
        {

        }

        else if (inputActions.PlayerActions.HotBarSlot2.IsPressed())
        {

        }

        else if (inputActions.PlayerActions.HotBarSlot3.IsPressed())
        {

        }

        else if (inputActions.PlayerActions.HotBarSlot4.IsPressed())
        {

        }

        else if (inputActions.PlayerActions.HotBarSlot5.IsPressed())
        {

        }

        else if (inputActions.PlayerActions.HotBarSlot6.IsPressed())
        {

        }

        else if (inputActions.PlayerActions.HotBarSlot7.IsPressed())
        {

        }

        else if (inputActions.PlayerActions.HotBarSlot8.IsPressed())
        {

        }

        else if (inputActions.PlayerActions.HotBarSlot9.IsPressed())
        {

        }

        else
        {

        }
    }
}