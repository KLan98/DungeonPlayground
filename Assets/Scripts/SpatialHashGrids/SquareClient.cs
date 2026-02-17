using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareClient : MonoBehaviour
{
    [SerializeField] private Demo demo;
    [SerializeField] private BoxCollider2D collider2D;
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
            return collider2D.size;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Client client = demo.grid.NewClient(position, dimensions);
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(demo.grid.GetCellIndex(position));
    }
}
