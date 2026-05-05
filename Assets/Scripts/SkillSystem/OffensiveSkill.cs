using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct OffensiveSkill
{
    public PrimaryKey Key;
    public float CritChance;
    public byte Damage;

    public OffensiveSkill(PrimaryKey key, float critChance, byte damage)
    {
        Key = key;
        CritChance = critChance;
        Damage = damage;
    }
}