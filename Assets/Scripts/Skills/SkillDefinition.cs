using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New skill def", menuName = "Skill/skillDef")]
public class SkillDefinition : ScriptableObject
{
    public SkillData data;

    public Skill CreateSkill()
    {
        ISkillEffect effect;

        switch (data.skillID)
        {
            case SkillID.WIND_TELEPORTATION:
                effect = new TeleportationEffect();
                break;
            //case SkillID.ELECTRIC_LIGHTNINGSTRIKE:

            //    break;
            default:
                throw new System.NotImplementedException();
        }

        return new Skill(data, effect);
    }
}