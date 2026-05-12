using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class EntitiesManager : MonoBehaviour
{
    private EntityStats[] statsTable; // lookup table for stats
    private static EntitiesManager instance;
    private byte entityID = 1; // entityID cannot be 0, if 0 then it is tile

    [Header("Debug")]
    [SerializeField] private List<GameObject> roomEntities; // all entities that are not tiles in a room
    [SerializeField] private List<Entity> aliveEntities;
    [SerializeField] private List<Entity> deadEntities;
    [SerializeField] private List<Client> targettedEntities;

    private void Awake()
    {
        if (instance != null && instance == this)
        {
            return;
        }

        instance = this;

        statsTable = new EntityStats[]
        {
            new EntityStats(){EntityType = EntityType.PLAYER, HitPoint = 100, Speed = 10, Strength = 10},
            new EntityStats(){EntityType = EntityType.MINOTAUR, HitPoint = 120, Speed = 10, Strength = 8},
            new EntityStats(){EntityType = EntityType.SLIME, HitPoint= 70, Speed = 5, Strength = 3},
            new EntityStats(){EntityType = EntityType.BOT, HitPoint = 60, Speed = 5, Strength = 4},
            new EntityStats(){EntityType = EntityType.BARBARIAN, HitPoint = 40, Speed = 5, Strength = 8}
        };

        roomEntities = new List<GameObject>();
        aliveEntities = new List<Entity>();
        deadEntities = new List<Entity>();
        targettedEntities = new List<Client>();
    }

    public static EntitiesManager GetInstance()
    {
        return instance;
    }

    //---------------------------------ROOM ENTITIES---------------------------------------
    public List<GameObject> GetRoomEntities()
    {
        return roomEntities;
    }

    public void AddRoomEntity(GameObject id)
    {
        roomEntities.Add(id);
    }

    public void RemoveRoomEntity(GameObject id)
    {
        roomEntities.Remove(id);
    }

    public void ClearRoomEntities()
    {
        roomEntities.Clear();
    }

    //------------------------------STATS TABLE--------------------------------------------
    public EntityStats[] GetStatsTable()
    {
        return statsTable;
    }

    public void AssignStats(EntityType entityType, GameObject gameObject)
    {
        for (int i = 0; i < statsTable.Length; i++)
        {
            if (statsTable[i].EntityType == entityType)
            {
                Entity entity = new Entity() { EntityStats = statsTable[i], GameObject = gameObject, EntityID = entityID };
                AddAliveEntity(entity);

                entityID++;
            }
        }
    }

    public int GetEntityHitPoint(EntityType entityID)
    {
        foreach (var entity in statsTable)
        {
            if (entity.EntityType == entityID)
            {
                return entity.HitPoint;
            }
        }

        Debug.LogWarning($"Cannot return hp for {entityID}");
        return 0;
    }

    public byte GetEntitySpeed(EntityType entityID)
    {
        foreach (var entity in statsTable)
        {
            if (entity.EntityType == entityID)
            {
                return entity.Speed;
            }
        }

        Debug.LogWarning($"Cannot return speed for {entityID}");
        return 0;
    }

    public void SetEntitySpeed(EntityType entityID, byte newValue)
    {
        for (int i = 0; i < statsTable.Length; i++)
        {
            if (statsTable[i].EntityType == entityID)
            {
                statsTable[i].Speed = newValue;
            }
        }
    }

    public void IncrementEntitySpeed(EntityType entityID)
    {
        for (int i = 0; i < statsTable.Length; i++)
        {
            if (statsTable[i].EntityType == entityID)
            {
                statsTable[i].Speed++;
            }
        }
    }

    //-------------------------------ALIVE ENTITIES---------------------------
    public List<Entity> GetAliveEntities()
    {
        return aliveEntities;
    }

    public void AddAliveEntity(Entity entity)
    {
        aliveEntities.Add(entity);
    }

    public void RemoveAliveEntity(Entity entity)
    {
        aliveEntities.Remove(entity);
    }

    public void ClearAliveEntity()
    {
        aliveEntities.Clear();
    }

    public int CountAliveEntities()
    {
        return aliveEntities.Count;
    }

    public byte GetEntityID(GameObject gameObject)
    {
        for (int i = 0; i < aliveEntities.Count; i++)
        {
            if (gameObject == aliveEntities[i].GameObject)
            {
                return aliveEntities[i].EntityID;
            }
        }

        Debug.LogWarning($"Cannot find the entityID for this {gameObject}");
        return 0;
    }

    public GameObject GetAliveGameObject(byte entityID)
    {
        for (int i = 0; i < aliveEntities.Count; i++)
        {
            if (entityID == aliveEntities[i].EntityID)
            {
                return aliveEntities[i].GameObject;
            }
        }

        Debug.LogWarning($"Cannot find the GameObject for this ID {entityID}");
        return null;
    }

    //-------------------------------DEAD ENTITIES---------------------------
    public List<Entity> GetDeadEntities()
    {
        return deadEntities;
    }

    public void AddDeadEntity(Entity entity)
    {
        deadEntities.Add(entity);
    }

    public void RemoveDeadEntity(Entity entity)
    {
        deadEntities.Remove(entity);
    }

    public void ClearDeadEntity()
    {
        deadEntities.Clear();
    }

    public int CountDeadEntities()
    {
        return deadEntities.Count;
    }

    //--------------------------TARGETTED ENTITIES-----------------------------
    public List<Client> GetTargettedEntities()
    {
        return targettedEntities;
    }

    public void AddTargettedEntity(Client entity)
    {
        targettedEntities.Add(entity);
    }

    public void RemoveTargettedEntity(Client entity)
    {
        targettedEntities.Remove(entity);
    }

    public void ClearTargettedEntities()
    {
        targettedEntities.Clear();
    }

    public int CountTargettedEntities()
    {
        return targettedEntities.Count;
    }
}

public enum EntityType
{
    PLAYER,
    MINOTAUR,
    SLIME,
    BOT,
    BARBARIAN,
}