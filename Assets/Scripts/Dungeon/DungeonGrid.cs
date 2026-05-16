using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGrid : MonoBehaviour
{
    readonly private Vector2[] bounds = new Vector2[] { new Vector2(-100, -100), new Vector2(100, 100) };
    readonly private Vector2 dimension = new Vector2(200, 200);
    readonly private Vector2Int[] _bounds = new Vector2Int[] { new Vector2Int(-100, -100), new Vector2Int(100, 100) };
    readonly private Vector2Int _dimension = new Vector2Int(200, 200);
    public static DungeonGrid Instance { get; private set; }
    public SpatialHashGridsOptimized spatialHashGrid;
    private SpatialHashGrid _spatialHashGrid;

    private void Awake()
    {
        spatialHashGrid = new SpatialHashGridsOptimized(bounds, dimension);
        _spatialHashGrid = new SpatialHashGrid(_bounds, _dimension);
        Instance = this;
    }

    //private void OnDrawGizmos()
    //{
    //    int minX = (int)bounds[0][0];
    //    int maxX = (int)bounds[1][0];

    //    int totalPoints = (int)dimension.x; 

    //    int diff = (maxX - minX) / totalPoints;

    //    Gizmos.color = Color.red;

    //    for (int i = -totalPoints / 2; i < totalPoints / 2; i++)
    //    {
    //        Vector2 startPoint = new Vector2(i * diff, minX);
    //        Vector2 endPoint = new Vector2(i * diff, maxX);
    //        Gizmos.DrawLine(startPoint, endPoint);
    //    }

    //    for (int i = -totalPoints / 2; i < totalPoints / 2; i++)
    //    {
    //        Vector2 startPoint = new Vector2(minX, diff * i);
    //        Vector2 endPoint = new Vector2(maxX, diff * i);
    //        Gizmos.DrawLine(startPoint, endPoint);
    //    }
    //}
    
    public SpatialHashGrid GetSpatialHashGrid()
    {
        return _spatialHashGrid;
    }

    public Client GetClientAtIndex(Vector2Int index)
    {
        Key key = new Key(index.x, index.y);
        if (spatialHashGrid.Cells.TryGetValue(key, out List<Client> clients))
        {
            foreach (Client client in clients)
            {
                if (!client.WalkableTile)
                {
                    return client;
                }
            }
        }

        return null;
    }

    public byte GetEntityIDAtIndex(Vector2Int index)
    {
        Key key = new Key(index.x, index.y);
        if (spatialHashGrid.Cells.TryGetValue(key, out List<Client> clients))
        {
            foreach (Client client in clients)
            {
                if (client.EntityID != 0)
                {
                    return client.EntityID;
                }
            }
        }

        return 0;
    }
}
