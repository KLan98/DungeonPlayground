using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class RepeatUntilFail : BehaviorTreeNode
{
    public RepeatUntilFail(string name, BehaviorTreeNode parentNode) : base(name, parentNode)
    {
    }

    public override TreeNodeState Evaluate()
    {
        if (childrenNodes.Count == 1)
        {
            while (childrenNodes[0].Evaluate() != TreeNodeState.FAILED)
            {
                //Debug.Log($"{this} is running");
                return TreeNodeState.RUNNING;
            }

            //Debug.Log($"{this} is passed");
            return TreeNodeState.PASSED;
        }

        return TreeNodeState.INVALID;
    }
}
