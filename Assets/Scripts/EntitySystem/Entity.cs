using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// used for alive/dead list
[System.Serializable]
public struct Entity
{
    public EntityStats EntityStats;
    public GameObject GameObject;
    public byte EntityID;

    public Entity(EntityStats entityStats, GameObject gameObject, byte entityID)
    {
        EntityStats = entityStats;
        GameObject = gameObject;
        EntityID = entityID;
    }
}
