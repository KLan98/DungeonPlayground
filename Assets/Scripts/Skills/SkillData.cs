using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Data needed to create a skill in editor
[System.Serializable]
public struct SkillData
{
    public SkillID SkillID;
    public bool IsChainable; // can be changed 
    public SkillTargetType SkillTargetType;
    public int NumberOfTargets; // valid for TARGET_TYPE_MULTIPLE_POINT_AND_CLICK
    public EffectType EffectType;
    public DisplayedText DisplayedText;
    public Damage Damage;
    public Cost Cost;
}

[System.Serializable]
public struct Damage
{
    public int baseDamage;
    public float damageMultiplier;

    public Damage(int baseDamage = 0, float damageMultiplier = 0.0f)
    {
        this.baseDamage = baseDamage;
        this.damageMultiplier = damageMultiplier;
    }
}

[System.Serializable]
public struct DisplayedText
{
    public string displayName;
    public string displayDescription;
}

[System.Serializable]
public struct Cost
{
    public int usageCost;
    public int upgradeCost;

    public Cost(int usageCost = 1, int upgradeCose = 0)
    {
        this.usageCost = usageCost;
        this.upgradeCost = upgradeCose;
    }
}