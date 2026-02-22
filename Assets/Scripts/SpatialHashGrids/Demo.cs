using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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

    private void Update()
    {
        
    }

    private void OnDrawGizmos()
    {
        const int minX = -1000;
        const int maxX = 1000;

        int totalPoints = 500;

        int diff = (maxX - minX) / totalPoints;

        Gizmos.color = Color.red;

        for (int i = -totalPoints / 2; i < totalPoints / 2; i++)
        {
            Vector2 startPoint = new Vector2(i * diff, minX);
            Vector2 endPoint = new Vector2(i * diff, maxX);
            Gizmos.DrawLine(startPoint, endPoint);
        }

        for (int i = -totalPoints / 2; i < totalPoints / 2; i++)
        {
            Vector2 startPoint = new Vector2(minX, diff * i);
            Vector2 endPoint = new Vector2(maxX, diff * i);
            Gizmos.DrawLine(startPoint, endPoint);
        }
    }
}
