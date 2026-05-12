using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
    public class EnemyBehaviorTree
    {
        private string name;
        private List<BehaviorTreeNode> behaviorTree = new List<BehaviorTreeNode>();

        public EnemyBehaviorTree(string name) 
        {
            this.name = name;
        }

        // add a node to this tree
        public void AddNode(BehaviorTreeNode node)
        {
            behaviorTree.Add(node);
        }

        /// <summary>
        /// Process this tree, called in game object update method, process the first behavior node
        /// </summary>
        public void Process()
        {
            if (behaviorTree.Count > 0)
            {
                // process the origin node
                TreeNodeState status = behaviorTree[0].Evaluate();
                Debug.Log($"status of the behavior tree = {status}");
            }
        }
    }
}
