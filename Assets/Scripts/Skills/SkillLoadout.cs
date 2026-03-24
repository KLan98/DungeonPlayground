using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
