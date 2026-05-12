using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

// invert the state of its child node
public class Invert : BehaviorTreeNode
{
    public Invert(string name, BehaviorTreeNode parentNode) : base(name, parentNode)
    {
    }

    public override TreeNodeState Evaluate()
    {
        if (childrenNodes.Count == 1)
        {
            switch (childrenNodes[0].Evaluate())
            {
                case TreeNodeState.PASSED:
                    //Debug.Log($"{this} returns failed");
                    return TreeNodeState.FAILED;
                case TreeNodeState.FAILED:
                    //Debug.Log($"{this} returns passed");
                    return TreeNodeState.PASSED;
                default:
                    //Debug.Log($"{this} is running");
                    return TreeNodeState.RUNNING;
            }
        }

        return TreeNodeState.INVALID;
    }
}
