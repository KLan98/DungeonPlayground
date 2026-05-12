using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
    public class ChasePlayerTree
    {
        private EnemyBehaviorTree tree;

        public ChasePlayerTree(Client client)
        {
            // create a tree
            tree = new EnemyBehaviorTree($"{this}");

            // top level node
            Sequence sequenceNode = new Sequence($"{this}" + " Sequence", null);

            // strategy for the leaf nodes
            ChasePlayer chasePlayerStrategy = new ChasePlayer(client, DungeonGrid.Instance.spatialHashGrid.Cells);
            MoveOneStep moveOneStepStrategy = new MoveOneStep(chasePlayerStrategy, client.EntityID);

            // leaf nodes
            Leaf findPlayerNode = new Leaf("FindPlayer", sequenceNode, chasePlayerStrategy);
            Leaf moveTowardPlayerNode = new Leaf("MoveTowardPlayer", sequenceNode, moveOneStepStrategy);

            tree.AddNode(sequenceNode);
            tree.AddNode(findPlayerNode);
            tree.AddNode(moveTowardPlayerNode);
        }

        public void ProcessTree()
        {
            tree.Process();
        }
    }
}
