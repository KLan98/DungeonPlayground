using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
    public class EnemyBehaviorTree
    {
        private string name;
        private List<BehaviorTreeNode> behaviorTreeNodes = new List<BehaviorTreeNode>();

        public EnemyBehaviorTree(string name) 
        {
            this.name = name;
        }

        // add a node to this tree
        public void AddNode(BehaviorTreeNode node)
        {
            behaviorTreeNodes.Add(node);
        }

        /// <summary>
        /// Process this tree, called in game object update method, process the first behavior node
        /// </summary>
        public void Process()
        {
            if (behaviorTreeNodes.Count > 0)
            {
                behaviorTreeNodes[0].Evaluate();
                Debug.Log($"status = {behaviorTreeNodes[0].Evaluate()}");
            }
        }
    }
}
