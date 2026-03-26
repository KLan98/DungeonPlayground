using System;
using UnityEngine;
using System.Collections.Generic;

public abstract class Skill
{
    public SkillData Data { get; private set; }

    public int Level { get; private set; }
    public SkillRequirements SkillRequirements { get; private set; }    

    public Skill(SkillData data) 
    { 
        this.Data = data; 
        Level = 1;
        SkillRequirements = new SkillRequirements(data);
    }

    public void Upgrade(SkillData upgradedData) 
    { 
        Data = upgradedData; 
        Level++;
        SkillRequirements = new SkillRequirements(upgradedData);
    }

    public abstract void CastSkill(SkillContext context);
}