using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MyCharacterController : MonoBehaviour
{
    [SerializeField] private UnitMotor motor;
    [SerializeField] private float speed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        motor.AttachedRigidbody2D.velocity = motor.MoveDirection * speed;
    }
}
