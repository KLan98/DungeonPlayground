using System;
using UnityEngine;

[System.Serializable]
public struct EntityStats
{
    public EntityType EntityType;
    public int HitPoint;
    public byte Speed;
    public byte Strength;

    public EntityStats(EntityType entityType, int hitPoint, byte speed, byte strength)
    {
        EntityType = entityType;
        HitPoint = hitPoint;
        Speed = speed;
        Strength = strength;
    }
}
