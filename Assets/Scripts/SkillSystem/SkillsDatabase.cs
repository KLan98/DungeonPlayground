using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[System.Serializable]
public class SkillsDatabase
{
    [SerializeField] private SkillElements[] skillElements;
    [SerializeField] private OffensiveSkill[] offensiveSkills;
    [SerializeField] private CrowdControlSkill[] crowdControlSkills;

    public SkillsDatabase()
    {
        InitSkillsList();
        InitOffensiveSkills();
        InitCrowdControlSkills();
    }

    private void InitSkillsList()
    {
        skillElements = new SkillElements[] {
            new SkillElements(Element.Fire, 1, 1),
            new SkillElements(Element.Neutral, 1, 1),
            new SkillElements(Element.Ice, 1, 1),
            new SkillElements(Element.Electric, 1, 1),
            
            // level 2 skills
            new SkillElements(Element.Neutral, 2, 0),
        };
    }

    private void InitOffensiveSkills()
    {
        offensiveSkills = new OffensiveSkill[] {
            new OffensiveSkill(new PrimaryKey(SkillID.BOMB, 1), 5.0f, 5),
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
    public SkillElements GetSkill(byte key)
    {
        return skillElements[key];
    }

    public OffensiveSkill GetOffensiveSkill(byte key)
    {
        return offensiveSkills[key];
    }

    public CrowdControlSkill GetCrowdControlSkill(byte key)
    {
        return crowdControlSkills[key];
    }
}