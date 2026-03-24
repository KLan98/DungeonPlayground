using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private DungeonGrid dungeonGrid;
    [SerializeField] List<Client> nearByClients = new List<Client>();
    private Client client;

    private Vector2 position
    {
        get { return this.gameObject.transform.position; }
    }

    private Vector2 dimension = new Vector2(0.9f, 0.9f);

    public void FindNearbyClients()
    {
        nearByClients.Clear();
        nearByClients.AddRange(dungeonGrid.spatialHashGrid.FindNear(position, dimension));
    }

    public void UpdateGrid()
    {
        dungeonGrid.spatialHashGrid.UpdateGrid(client);
    }

    // Start is called before the first frame update
    void Start()
    {
        sprite = gameObject.GetComponentInChildren<SpriteRenderer>();
        client = dungeonGrid.spatialHashGrid.NewClient(position, dimension, "Player");
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
}
