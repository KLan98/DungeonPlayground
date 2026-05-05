using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Modifier for bomb explode
/// </summary>
public class ModifierBombExplode : Modifiers
{
    public override void OnCreated(ThinkerParams thinkerParams)
    {
        this.delay = thinkerParams.Delay;
        this.blastRadius = thinkerParams.BlastRadius;
        this.damage = thinkerParams.Damage;
        this.index = thinkerParams.Index;
        this.level = thinkerParams.Level;
        Debug.Log($"modifier for bomb explode created with thinkerParams {thinkerParams}");
        Invoke(nameof(OnThinkOnce), delay);
    }

    protected override void OnThinkOnce()
    {
        if (blastRadius == 0)
        {
            Debug.Log("This modifier requires greater than 0 blast radius");
            return;
        }

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

    protected override List<Client> FindTargetsInBlastRadius(Vector2Int cursorIndex, int maxRadius)
    {
        List<Client> hitTargets = new List<Client>();
        for (int x = -maxRadius; x <= maxRadius; x++)
        {
            for (int y = -maxRadius; y <= maxRadius; y++)
            {
                int manhattanDistance = Mathf.Abs(x) + Mathf.Abs(y);

                // only considered as hit target whenever
                // stopping player to cast this skill at his position
                if (manhattanDistance > 0 && manhattanDistance <= maxRadius)
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
