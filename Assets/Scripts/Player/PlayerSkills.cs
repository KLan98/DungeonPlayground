using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerSkills
{
    public List<PrimaryKey> CastableSkills;

    public void AddSkill(PrimaryKey key)
    { 
        CastableSkills.Add(key);
    }

    public void RemoveSkill(PrimaryKey key)
    {
        CastableSkills.Remove(key);
    }
}
