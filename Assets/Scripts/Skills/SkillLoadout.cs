using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents a collection of Skill instances created from SkillDefinition objects.
/// Provides safe access to skills by index, including bounds checking.
/// </summary>
public class SkillLoadout
{
    private List<Skill> skillsList = new List<Skill>();

    public SkillLoadout(IEnumerable<SkillDefinition> definitions)
    {
        foreach (var definition in definitions)
        {
            skillsList.Add(definition.CreateSkill());
        }
    }

    public Skill GetSkill(int index)
    {
        if (index < 0 || index >= skillsList.Count)
        {
            Debug.LogError("SkillLoadout.GetSkill: index " + index + " is out of range. Loadout has " + skillsList.Count + " skills.");
            return null;
        }

        return skillsList[index];
    }
}
