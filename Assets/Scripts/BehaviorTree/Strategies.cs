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
                //Debug.Log($"{this} returns failed");
                return TreeNodeState.FAILED;
            }

            else
            {
                //Debug.Log($"{this} returns passed");
                return TreeNodeState.PASSED;
            }
        }

        public Client GetTile()
        {
            if (myPath.Count >= 1)
            {
                return myPath[0];
            }

            return null;
        }
    }

    public class MoveOneStep : IStrategy
    {
        private ChasePlayer chasePlayerStrategy;
        private byte entityID;

        public MoveOneStep(ChasePlayer chasePlayerStrategy, byte entityID)
        {
            this.chasePlayerStrategy = chasePlayerStrategy;
            this.entityID = entityID;
        }

        public TreeNodeState Process()
        {
            int distanceToPlayer = chasePlayerStrategy.GetTile().DistanceToPlayer;
            Vector2 newPosition = chasePlayerStrategy.GetTile().Position;

            EntitiesManager.GetInstance().GetAliveGameObject(entityID).transform.position = newPosition;

            TilemapVisualizer.GetInstance().ColorTileByDistance(newPosition, distanceToPlayer);
            //Debug.Log($"{this} returns passed");
            return TreeNodeState.PASSED;
        }
    }
}