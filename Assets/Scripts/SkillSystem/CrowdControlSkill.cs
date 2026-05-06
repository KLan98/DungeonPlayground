using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct CrowdControlSkill
{
    public PrimaryKey PrimaryKey;
    public byte BlastRadius;

    public CrowdControlSkill(PrimaryKey primaryKey, byte blastRadius)
    {
        PrimaryKey = primaryKey;
        BlastRadius = blastRadius;
    }
}
