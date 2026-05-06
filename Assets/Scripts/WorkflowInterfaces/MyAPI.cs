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
    public static void CreateThinker(PrimaryKey primaryKey, Vector2 origin)
    {
        GameObject thinker = new GameObject(primaryKey.SkillID.ToString() + " THINKER");
        thinker.transform.position = origin;

        switch (primaryKey.SkillID)
        {
            case SkillID.BOMB:
                thinker.AddComponent<ModifierBombExplode>().OnCreated(primaryKey);
                break;
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

public struct DamageTable
{
    public Client[] Victims;
    public Vector2Int Direction;
    public Vector2 Position;
    public byte Damage;
}
