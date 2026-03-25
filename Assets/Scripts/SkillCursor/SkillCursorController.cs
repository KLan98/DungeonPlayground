using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillCursorController : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private DungeonGrid dungeonGrid;
    [SerializeField] List<Client> nearByClients = new List<Client>();
    private Client client;

    private Vector2 position
    {
        get {  return gameObject.transform.position; }
    }

    private Vector2 dimension = new Vector2(0.9f, 0.9f);
    
    private void Start()
    {
        client = dungeonGrid.spatialHashGrid.NewClient(position, dimension, "SkillCursor");
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawWireCube(position, dimension);
    }

    private void Update()
    {
        FindNearbyClients();
        UpdateGrid();
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
}