using PathFinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
using Unity.VisualScripting;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private Client client;
    [SerializeField] private DungeonGrid dungeonGrid;
    [SerializeField] private TilemapVisualizer tilemapVisualizer;
    [SerializeField] private List<Client> nearByClients = new List<Client>();
    [SerializeField] private ChasePlayer chasePlayerStrategy;
    private MoveOneStep moveOneStepStrategy;

    private RepeatUntilFail repeatUntilFailNode;
    private Invert invertNode;
    private Sequence sequenceNode;
    private Leaf findPlayerNode;
    private Leaf moveTowardPlayerNode;
    private EnemyBehaviorTree tree;

    private EntityType entityType;

    private Vector2 position
    {
        get { return this.gameObject.transform.position; }
    }

    private Vector2 dimension = new Vector2(0.9f, 0.9f);

    //--------------------------EVENT RESPONSE--------------------------------
    public void FindNearbyClients()
    {
        nearByClients.Clear();
        foreach (var c in dungeonGrid.spatialHashGrid.FindNear(position, dimension))
        {
            if (c.ClientID != client.ClientID)
                nearByClients.Add(c);
        }
    }

    // LAN_TODO called whenever the enemy position changed, hasn't been used now
    public void UpdateGrid()
    {
        UpdateClientInfo();
        dungeonGrid.spatialHashGrid.UpdateGrid(client);
    }

    public void OnEnemyMove()
    {
        FindNearbyClients();
        UpdateGrid();
    }


    //-------------------------------PRIVATE METHODS-------------------------------
    private void UpdateClientInfo()
    {
        client.Position = position;
    }

    public void ProcessBehaviorTree()
    {
        tree.Process();
    }

    //--------------------------BUILT-IN METHODS--------------------------------    
    private void Awake()
    {
        entityType = EntityType.BARBARIAN;
    }

    // Start is called before the first frame update
    void Start()
    {

        //EntitiesManager.GetInstance().AddRoomEntity(this.gameObject);

        // ref to statsTable
        EntitiesManager.GetInstance().AssignStats(entityType, this.gameObject);

        byte entityID = EntitiesManager.GetInstance().GetEntityID(this.gameObject);

        client = dungeonGrid.spatialHashGrid.NewClient(position, dimension, "Actor", false, entityID);
        client.GameObject = this.gameObject;

        //----------------------TREE CONSTRUCTION, ORDER MATTERS-----------------------
        chasePlayerStrategy = new ChasePlayer(client, dungeonGrid.spatialHashGrid.Cells);
        moveOneStepStrategy = new MoveOneStep();

        sequenceNode = new Sequence("MySequence", null);
        repeatUntilFailNode = new RepeatUntilFail("MyRepeatUntilFail", sequenceNode);
        moveTowardPlayerNode = new Leaf("MoveToPlayer", sequenceNode, moveOneStepStrategy);

        invertNode = new Invert("MyInvert", repeatUntilFailNode);
        findPlayerNode = new Leaf("FindPlayer", invertNode, chasePlayerStrategy);

        tree = new EnemyBehaviorTree("Actor behavior tree");
        tree.AddNode(sequenceNode);
        tree.AddNode(repeatUntilFailNode);
        tree.AddNode(moveTowardPlayerNode);
        tree.AddNode(invertNode);
        tree.AddNode(findPlayerNode);

        transform.position = MyAPI.GetCellCenter(position);

        OnEnemyMove();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawWireCube(position, dimension);
    }
}
