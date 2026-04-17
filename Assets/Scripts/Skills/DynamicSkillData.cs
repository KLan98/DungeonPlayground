using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct SkillDynamicData 
{
    public Vector2Int DestinationIndex;
    public List<Vector2Int> ImpactRangeIndex; // the collection of impact index based on the destination index
}
