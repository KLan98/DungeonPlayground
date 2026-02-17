using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
// Handle movement of units between ogPos and targetPos
public class UnitMotor : MonoBehaviour
{
    [SerializeField] private Transform originalPosition;
    [SerializeField] private Transform targetPosition;
    private Rigidbody2D rg2d;
    private BoxCollider2D col2D;

    public Transform OriginalPosition
    {
        get { return originalPosition; }
    }

    public Transform TargetPosition
    {
        get { return targetPosition; } 
    }

    public Vector2 BaseVelocity
    {
        get { return rg2d.velocity; }
    }

    public Rigidbody2D AttachedRigidbody2D
    {
        get { return rg2d; } 
    }

    public Transform Transform
    {
        get { return transform; }
    }

    public Vector2 MoveDirection
    {
        get { return TargetPosition.position - OriginalPosition.position; }
    }

    private void Start()
    {
        rg2d = GetComponent<Rigidbody2D>();
        col2D = GetComponent<BoxCollider2D>();
    }

    public void SetPositionAndRotation(Vector3 position, Quaternion rotation)
    {

    }

    public void SetPosition(Vector3 position)
    {
    }

    public void SetTargetPosition(Vector3 targetPosition)
    {

    }

    public void SetOriginalPosition(Vector3 originalPosition)
    {

    }

    public void MoveUnit(Vector3 toPosition)
    {

    }

    public void RotateUnit(Quaternion toRotation)
    {

    }

    //public int CharacterCollisionsRaycast(Vector3 position, Vector3 direction, float distance,out RaycastHit closestHit, RaycastHit[] hits)
    //{
    //    closestHit = 0;
    //    return ;
    //}
}
