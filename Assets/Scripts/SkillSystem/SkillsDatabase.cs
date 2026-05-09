using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[System.Serializable]
public class SkillsDatabase
{
    [SerializeField] private CostTable[] costTable;
    [SerializeField] private OffensiveSkill[] offensiveSkills;
    [SerializeField] private CrowdControlSkill[] crowdControlSkills;
    // elemental rela table 

    public SkillsDatabase()
    {
        InitSkillsElementList();
        InitOffensiveSkills();
        InitCrowdControlSkills();
    }

    private void InitSkillsElementList()
    {
        costTable = new CostTable[] {
            new CostTable(1, 0),
            new CostTable(1, 1),
            new CostTable(2, 0),
            new CostTable(2, 1),
            new CostTable(1, 2), // only usable when skills are chained
        };
    }

    private void InitOffensiveSkills()
    {
        offensiveSkills = new OffensiveSkill[] {
            new OffensiveSkill(new PrimaryKey(SkillID.BOMB, 1), 5.0f, 100),
            new OffensiveSkill(new PrimaryKey(SkillID.ZAP, 1), 10.0f, 5),
        };
    }

    private void InitCrowdControlSkills()
    {
        crowdControlSkills = new CrowdControlSkill[] {
            new CrowdControlSkill(new PrimaryKey(SkillID.BOMB, 1), 1),
            new CrowdControlSkill(new PrimaryKey(SkillID.FROZA, 1), 1),
        };
    }

    //----------------------------PUBLIC METHODS--------------------
    public CostTable GetCostTable(byte key)
    {
        return costTable[key];
    }

    public OffensiveSkill GetOffensiveSkill(PrimaryKey key)
    {
        foreach (var skill in offensiveSkills)
        {
            if (skill.PrimaryKey.Equals(key))
            {
                return skill;
            }
        }

        throw new InvalidOperationException($"{key} not found in {offensiveSkills} collection");
    }

    public CrowdControlSkill GetCrowdControlSkill(PrimaryKey key)
    {
        foreach (var skill in crowdControlSkills)
        {
            if (skill.PrimaryKey.Equals(key))
            {
                return skill;
            }
        }

        throw new InvalidOperationException($"{key} not found in {crowdControlSkills} collection");
    }
}