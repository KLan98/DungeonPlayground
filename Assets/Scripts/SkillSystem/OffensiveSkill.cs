using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct OffensiveSkill
{
    public PrimaryKey PrimaryKey;
    public float CritChance;
    public byte Damage;

    public OffensiveSkill(PrimaryKey PrimaryKey, float critChance, byte damage)
    {
        this.PrimaryKey = PrimaryKey;
        CritChance = critChance;
        Damage = damage;
    }
}