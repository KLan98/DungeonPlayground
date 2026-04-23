using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyAPI
{
    /// <summary>
    /// Thinker contains modifiers here is where modifiers for skills are added. A skill need modifiers to behave correctly
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

public struct ThinkerParams
{
    public int Damage;
    public int MaxRange;
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
