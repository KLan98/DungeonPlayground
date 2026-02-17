using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Demo : MonoBehaviour
{
    readonly private Vector2[] bounds = new Vector2[] { new Vector2(-1000, -1000), new Vector2(1000, 1000) };
    readonly private Vector2 dimension = new Vector2(500, 500); // 500 x 500 grid 

    public SpatialHashGrids grid;

    private void Awake()
    {
        grid = new SpatialHashGrids(bounds, dimension);
    }

    private void Start()
    {
    }
}
