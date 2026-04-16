using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
    public interface IStrategy
    {
        // the process of this strategy
        TreeNodeState Process();

        // reset the internal variable of the strategy
        void Reset();
    }

    public class PatrolStrategy : IStrategy
    {
        readonly Client actor;
        readonly List<Transform> patrolPoints = new List<Transform>();
        private int currentIndex; // index of current patrol point
        private bool isMoving = false;

        public PatrolStrategy(Client actor, List<Transform> patrolPoints, int currentIndex) 
        {
            this.actor = actor;
            this.patrolPoints = patrolPoints;
            this.currentIndex = currentIndex;
        }

        public TreeNodeState Process()
        {
            if (currentIndex == patrolPoints.Count - 1)
            {
                return TreeNodeState.PASSED;
            }

            // LAN_TODO, code the process for this strategy, if Process is in update function then it wouldn't be efficient
            return TreeNodeState.RUNNING;
        }

        public void Reset()
        {
            currentIndex = 0;
        }
    }
}
