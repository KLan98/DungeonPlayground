using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace BehaviorTree
{
    public enum TreeNodeState
    {
        PASSED,
        FAILED,
        RUNNING, // FOR NODES THAT IS RUNNING
        INVALID // INVALID FLAG
    }

    public class BehaviorTreeNode
    {
        protected string name;

        protected BehaviorTreeNode parentNode;

        protected List<BehaviorTreeNode> childrenNodes = new List<BehaviorTreeNode>();

        private Dictionary<string, object> data = new Dictionary<string, object>(); // data for this node

        public string Name
        {
            get { return name; }
        }

        public BehaviorTreeNode(string name, BehaviorTreeNode parentNode = null)
        {
            this.name = name;
            this.parentNode = parentNode;
            AttachToParent(this);
        }

        private void AttachToParent(BehaviorTreeNode childNode)
        {
            childNode.parentNode = parentNode;

            if (parentNode != null)
            {
                parentNode.childrenNodes.Add(this);
            }
        }

        public void SetData(string key, object value)
        {
            data[key] = value;
        }
        
        // get the data from this node
        public object GetData(string key)
        {
            if (data.TryGetValue(key, out object value))
            {
                return value;
            }

            return null;   
        }

        public virtual TreeNodeState Evaluate()
        {
            return TreeNodeState.RUNNING;
        }
    }
}