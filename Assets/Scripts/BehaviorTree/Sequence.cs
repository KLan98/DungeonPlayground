using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class Sequence : BehaviorTreeNode
{
    public Sequence(string name, BehaviorTreeNode parentNode) : base(name, parentNode)
    {
        
    }

    public override TreeNodeState Evaluate()
    {
        // if a child failed then return failed
        // if a child is running then return running
        // if childrenNodes <= 1 then invalid
        if (childrenNodes.Count <= 1)
        {
            return TreeNodeState.INVALID;
        }

        foreach (var childNode in childrenNodes)
        {
            //Debug.Log($"evaluating {childNode.Name}");
            switch (childNode.Evaluate())
            {
                case TreeNodeState.RUNNING:
                    //Debug.Log($"{this} is running");
                    return TreeNodeState.RUNNING;
                case TreeNodeState.FAILED:
                    //Debug.Log($"{this} returns failed");
                    return TreeNodeState.FAILED;
            }
        }

        //Debug.Log($"{this} returns passed");
        // if all children passed then return passed
        return TreeNodeState.PASSED;
    }
}
