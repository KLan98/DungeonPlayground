using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

// a leaf has its own strategy
public class Leaf : BehaviorTreeNode
{
    private readonly IStrategy strategy;

    public Leaf(string name, BehaviorTreeNode parentNode, IStrategy strategy) : base(name, parentNode)
    {
        this.strategy = strategy;
    }

    // the return of evaluate is the result of strategy process
    public override TreeNodeState Evaluate()
    {
        return strategy.Process();
    }
}
