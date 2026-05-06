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

    public override void OnCreated(PrimaryKey primaryKey)
    {
        this.primaryKey = primaryKey;
        Debug.Log($"{this} created");
        Invoke(nameof(OnThinkOnce), 0.5f);
    }

    protected override void OnThinkOnce()
    {
        byte blastRadius = GameManager.GetInstance().GetSkillsDatabase().GetCrowdControlSkill(primaryKey).BlastRadius;
        byte damage = GameManager.GetInstance().GetSkillsDatabase().GetOffensiveSkill(primaryKey).Damage;
        float critChance = GameManager.GetInstance().GetSkillsDatabase().GetOffensiveSkill(primaryKey).CritChance;
        //Element element = GameManager.GetInstance().GetSkillsDatabase().GetSkillElement(0).Element;
        Vector2Int index = SkillCursorController.Instance.GetCursorIndex();
        byte level = primaryKey.Level;

        // find hit enemies 
        List<Client> hitTargets = FindTargetsInBlastRadius(index, blastRadius);

        // make each enemy takes damage
        foreach (Client target in hitTargets)
        {
            // can add elemental resistance
            if (target != null && !target.WalkableTile)
            {
                // construct damage table
                //DamageTable damageTable = {}
                DamageTable damageTable = new DamageTable();
                MyAPI.ApplyDamage(damageTable); // damage applied once
                Debug.Log($"{target.Name} hit with bomb at {target.Position}, dealing {damage} damage");
            }
        }

        // vfx
        GameObject effectInstance = EffectsManager.CreateEffect(Resources.Load<GameObject>("Prefabs/Effects/Explosion_LV" + level.ToString()), EffectAttach.ATTACH_WORLD, null);
        EffectsManager.SetEffectControl(effectInstance, this.transform.position);

        // sfx
    }

    protected override List<Client> FindTargetsInBlastRadius(Vector2Int cursorIndex, int blastRadius)
    {
        List<Client> hitTargets = new List<Client>();
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
                    
                    Client client = DungeonGrid.Instance.GetClientAtIndex(index);

                    if (client != null)
                    {
                        hitTargets.Add(client);    
                    }
                }
            }
        }

        return hitTargets;
    }
}
