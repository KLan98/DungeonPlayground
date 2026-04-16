using PathFinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
    public interface IStrategy
    {
        // the process of this strategy
        TreeNodeState Process();
    }

    [System.Serializable]
    public class ChasePlayer : IStrategy
    {
        private Client actor;
        private Dictionary<Key, List<Client>> cells = new Dictionary<Key, List<Client>>();
        [SerializeField] private List<Client> myPath = new List<Client>();

        public ChasePlayer(Client actor, Dictionary<Key, List<Client>> cells) 
        {
            this.actor = actor;
            this.cells = cells;
        }

        public TreeNodeState Process()
        {
            // run pathfinding 
            // if somehow the destination could not be reached then return failed
            BFSPathFinding.PathFinding(actor, cells, myPath);
            if (myPath.Count < 1)
            {
                return TreeNodeState.FAILED;
            }

            else
            {
                return TreeNodeState.PASSED;
            }
        }
    }

    // LAN_TODO implement move one step strategy
    // probably need the mypath from chase player strategy, which is in the leaf node 
    [System.Serializable]
    public class MoveOneStep : IStrategy
    {
        public TreeNodeState Process()
        {
            return TreeNodeState.PASSED;
        }
    }
}