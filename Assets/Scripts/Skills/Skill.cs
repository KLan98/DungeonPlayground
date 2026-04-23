using System;
using UnityEngine;
using System.Collections.Generic;

public abstract class Skill
{
    private SkillData skillData;
    public SkillData SkillData
    {
        get
        {
            return skillData;
        }
    }

    private SkillDynamicData? dynamicData;
    public SkillDynamicData? DynamicData 
    {
        get
        {
            return dynamicData;
        }
    }

    private int level;
    public int Level 
    { 
        get
        {
            return level;
        }
    }

    private SkillRequirements skillRequirement;
    public SkillRequirements SkillRequirements 
    { 
        get
        {
            return skillRequirement;
        }
    }    

    public Skill(SkillData data, SkillDynamicData? dynamicData = null) 
    { 
        this.skillData = data; 
        this.dynamicData = dynamicData;
        this.level = 1;
        this.skillRequirement = new SkillRequirements(data);
    }

    // LAN_TODO, realize this upgrade feature, hasn't been used yet
    public void Upgrade(SkillData upgradedData) 
    { 
        skillData = upgradedData; 
        level++;
        skillRequirement = new SkillRequirements(upgradedData);
    }

    public abstract void CastSkill(SkillContext context);

    // Override this in any skill that has an area preview
    public virtual List<Vector2Int> GetImpactPattern(Vector2Int hoveredIndex)
    {
        return null;
    }
}