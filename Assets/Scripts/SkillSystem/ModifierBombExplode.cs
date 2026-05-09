using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Modifier for bomb explode
/// </summary>
public class ModifierBombExplode : Modifiers
{
    private PrimaryKey primaryKey;
    private List<byte> damageTable; // contain the ID of entities to deal damage

    public override void OnCreated(PrimaryKey primaryKey)
    {
        this.primaryKey = primaryKey;
        Debug.Log($"{this} created");
        Invoke(nameof(OnThinkOnce), 0.5f);

        damageTable = new List<byte>();
    }

    protected override void OnThinkOnce()
    {
        byte blastRadius = GameManager.GetInstance().GetSkillsDatabase().GetCrowdControlSkill(primaryKey).BlastRadius;
        byte damage = GameManager.GetInstance().GetSkillsDatabase().GetOffensiveSkill(primaryKey).Damage;
        float critChance = GameManager.GetInstance().GetSkillsDatabase().GetOffensiveSkill(primaryKey).CritChance;
        //Element element = GameManager.GetInstance().GetSkillsDatabase().GetSkillElement(0).Element;
        Vector2Int index = SkillCursorController.Instance.GetCursorIndex();
        byte level = primaryKey.Level;

        // find hit entities 
        List<byte> hitEntities = FindHitEntities(index, blastRadius);

        // constuct a damage table
        foreach (byte entityID in hitEntities)
        {
            // construct damage table
            damageTable.Add(entityID);
        }

        MyAPI.ApplyDamage(damageTable, damage); // damage applied once

        // vfx
        GameObject effectInstance = EffectsManager.CreateEffect(Resources.Load<GameObject>("Prefabs/Effects/Explosion_LV" + level.ToString()), EffectAttach.ATTACH_WORLD, null);
        EffectsManager.SetEffectControl(effectInstance, this.transform.position);

        // sfx
    }

    protected override List<byte> FindHitEntities(Vector2Int cursorIndex, int blastRadius)
    {
        List<byte> hitEntities = new List<byte>();
        for (int x = -blastRadius; x <= blastRadius; x++)
        {
            for (int y = -blastRadius; y <= blastRadius; y++)
            {
                int manhattanDistance = Mathf.Abs(x) + Mathf.Abs(y);

                // only considered as hit target whenever
                // stopping player to cast this skill at his position
                if (manhattanDistance > 0 && manhattanDistance <= blastRadius)
                {
                    Vector2Int index = new Vector2Int(x + cursorIndex.x, y + cursorIndex.y);
                    
                    byte entityID = DungeonGrid.Instance.GetEntityIDAtIndex(index);
                    hitEntities.Add(entityID);
                }
            }
        }

        return hitEntities;
    }
}
