using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyAPI
{
    /// <summary>
    /// Thinker contains modifiers here is where modifiers for skills are added. A skill need modifiers to have behaviors
    /// </summary>
    /// <param name="skillID"></param>
    /// <param name="origin"></param>
    /// <param name="thinkerParams"></param>
    /// <param name="grid"></param>
    public static void CreateThinker(SkillID skillID, Vector2 origin, ThinkerParams thinkerParams, DungeonGrid grid)
    {
        GameObject thinker = new GameObject(skillID.ToString() + " THINKER");
        thinker.transform.position = origin;
    
        switch(skillID)
        {
            case SkillID.BOMB:
                thinker.AddComponent<ModifierBombExplode>().OnCreated(thinkerParams, grid);
                break;
            case SkillID.WIND_TELEPORTATION:
                break;
        }
    }

    public static void ApplyDamage(DamageTable damageTable)
    {
        // LAN_TODO raise apply damage event, pass damage table to channel
    }
}

/// <summary>
/// Parameters used to configure a thinker instance. All parameters are built-in or primitive types
///
/// Damage: The amount of damage the skill will deal when applied.
///
/// MaxRange: The maximum Manhattan distance (|x1 - x2| + |y1 - y2|)
/// that the skill can reach from its origin.
///
/// Delay: The time delay (in seconds) before the modifier's
/// OnIntervalThink method is triggered.
///
/// Index: The grid cell index of the cursor
/// </summary>
public struct ThinkerParams
{
    public int Damage;
    public int BlastRadius;
    public float Delay;
    public Vector2Int Index;
}

public struct DamageTable
{
    // public Client Victim;
    public Vector2Int Direction;
    public Vector2 Position;
    public int Damage;
    // public DamageElement ...
}
