using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Data needed to create a skill in editor
[System.Serializable]
public struct SkillData
{
    public SkillID skillID;
    public SkillTargetType skillTargetType;
    public int numberOfTargets; // valid for TARGET_TYPE_MULTIPLE_POINT_AND_CLICK
    public EffectType effectType;
    public DisplayedText displayedText;
    public Timings timings;
    public Damage damage;
    public Cost cost;
}

[System.Serializable]
public struct Timings
{
    public float skillExecutionTime; // duration in seconds that the skill is being executed, from start casting skill until casting is finished
    public int skillDurationTurns; // duration in turns that the skill is being executed, 1 means only this turn
    public int coolDownTurns; // turns until skill cool down

    public Timings(float skillExecutionTime = 1, int skillDurationTurns = 1, int coolDownTurns = 0)
    {
        this.skillExecutionTime = skillExecutionTime;   
        this.skillDurationTurns = skillDurationTurns;   
        this.coolDownTurns = coolDownTurns;
    }
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