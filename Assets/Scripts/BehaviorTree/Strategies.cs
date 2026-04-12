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
        readonly GameObject entity;
        readonly List<Transform> patrolPoints = new List<Transform>();
        int currentIndex; // index of current patrol point

        public PatrolStrategy(GameObject entity, List<Transform> patrolPoints, int currentIndex) 
        {
            this.entity = entity;
            this.patrolPoints = patrolPoints;
            this.currentIndex = currentIndex;
        }

        public TreeNodeState Process()
        {
            if (currentIndex == patrolPoints.Count - 1)
            {
                return TreeNodeState.PASSED;
            }

            // use breath-first to find the shortest path to target
            return TreeNodeState.RUNNING;
        }

        public void Reset()
        {
            currentIndex = 0;
        }
    }
}
