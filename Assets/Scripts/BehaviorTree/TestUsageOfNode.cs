using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

// try to construct a behavior tree
public class TestUsageOfNode : MonoBehaviour
{
    private RepeatUntilFail repeatUntilFailNode;
    private Invert invertNode;
    //private Sequence sequenceNode;
    private Leaf patrolNode;

    // strategies
    private IStrategy patrolStrategy;

    //[SerializeField] GameObject entity;
    [SerializeField] List<Transform> wayPoints = new List<Transform>();
    [SerializeField] int currentIndex = 0;

    private void Awake()
    {
        // LAN_TODO need to add check so that patrol strategy is correctly constructed
        //patrolStrategy = new PatrolStrategy(entity, wayPoints, currentIndex);

        repeatUntilFailNode = new RepeatUntilFail("MyRepeatUntilFail", null);
        invertNode = new Invert("MyInvert", repeatUntilFailNode);
        //sequenceNode = new Sequence("MySequence", invertNode);
        patrolNode = new Leaf("Patrol", invertNode, patrolStrategy);
    }

    //private void Update()
    //{
    //    Debug.Log($"{patrolNode.Name} " + patrolNode.Evaluate());

    //    Debug.Log($"{repeatUntilFailNode.Name} " + repeatUntilFailNode.Evaluate());
    //}
}
