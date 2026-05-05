using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyAPI
{
    private const int tileSize = 1;
    /// <summary>
    /// Thinker contains modifiers here is where modifiers for skills are added. A skill need modifiers to have behaviors. Thinker is created OnSkillStart
    /// </summary>
    /// <param name="skillID"></param>
    /// <param name="origin"></param>
    /// <param name="thinkerParams"></param>
    public static void CreateThinker(SkillID skillID, Vector2 origin, ThinkerParams thinkerParams)
    {
        GameObject thinker = new GameObject(skillID.ToString() + " THINKER");
        thinker.transform.position = origin;

        switch (skillID)
        {
            case SkillID.BOMB:
                thinker.AddComponent<ModifierBombExplode>().OnCreated(thinkerParams);
                break;
            //case SkillID.TELEPORTATION:
            //   thinker.AddComponent<M_TeleportationTargetToDestination>().OnCreated(thinkerParams);
            //    break;
        }
    }

    public static void ApplyDamage(DamageTable damageTable)
    {
        // LAN_TODO raise apply damage event, pass damage table to channel
    }

    /// <summary>
    /// Take in world position and convert to cell
    /// </summary>
    /// <param name="worldPosition"></param>
    /// <returns></returns>
    public static Vector2 GetCellCenter(Vector2 worldPosition)
    {
        int xIndex = Mathf.FloorToInt(worldPosition.x / tileSize);
        int yIndex = Mathf.FloorToInt(worldPosition.y / tileSize);
        return new Vector2(
            xIndex * tileSize + tileSize / 2f,
            yIndex * tileSize + tileSize / 2f
        );
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
    public byte Damage;
    public byte BlastRadius;
    public float Delay;
    public Vector2Int Index;
    public byte Level;
}

public struct DamageTable
{
    // public Client Victim;
    public Vector2Int Direction;
    public Vector2 Position;
    public byte Damage;
    // public DamageElement ...
}
