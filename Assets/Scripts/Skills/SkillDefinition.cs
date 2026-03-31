using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New skill def", menuName = "Skill/skillDef")]
public class SkillDefinition : ScriptableObject
{
    public SkillData data;

    public Skill CreateSkill()
    {
        switch (data.skillID)
        {
            case SkillID.WIND_TELEPORTATION:
                return new Teleportation(data);
            case SkillID.ELECTRIC_LIGHTNINGCHAIN:
                return new LightningChain(data);
            default:
                throw new System.NotImplementedException();
        }
    }
}