using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct CrowdControlSkill
{
    public PrimaryKey Key;
    public byte BlastRadius;

    public CrowdControlSkill(PrimaryKey key, byte blastRadius)
    {
        Key = key;
        BlastRadius = blastRadius;
    }
}
