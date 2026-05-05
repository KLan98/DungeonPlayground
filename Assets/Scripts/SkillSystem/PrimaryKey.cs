using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct PrimaryKey 
{
    public SkillID SkillID;
    public byte Level;

    public PrimaryKey(SkillID skillID, byte level)
    {
        SkillID = skillID;
        Level = level;
    }
}
