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
    private BoxCollider2D collider2D;
    private Vector3 baseVelocity;

    public Transform OriginalPosition
    {
        get { return originalPosition; }
        set { originalPosition = value; }
    }

    public Transform TargetPosition
    {
        get { return targetPosition; } 
        set { targetPosition = value; } 
    }

    public Vector3 BaseVelocity
    {
        get { return baseVelocity; }
    }

    public Rigidbody2D AttachedRigidbody2D
    {
        get { return rg2d; } 
    }

    public Transform Transform
    {
        get { return transform; }
    }

    private void Start()
    {
        rg2d = GetComponent<Rigidbody2D>();
        collider2D = GetComponent<BoxCollider2D>();
    }

    public void SetPositionAndRotation(Vector3 position, Quaternion rotation)
    {

    }

    public void SetPosition(Vector3 position)
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
