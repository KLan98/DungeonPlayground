using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New skill def", menuName = "Skill/skillDef")]
public class SkillDefinition : ScriptableObject
{
    public SkillData data;

    public Skill CreateSkill()
    {
        switch (data.SkillID)
        {
            case SkillID.WIND_TELEPORTATION:
                return new Teleportation(data);
            case SkillID.ELECTRIC_LIGHTNINGCHAIN:
                return new LightningChain(data);
            case SkillID.BOMB:
                return new Bomb(data);
            default:
                throw new System.NotImplementedException();
        }
    }
}