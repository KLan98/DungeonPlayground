using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Modifier for bomb explode
/// </summary>
public class ModifierBombExplode : MonoBehaviour
{
    private float delay;
    private int blastRadius;
    private int damage;
    private Vector2Int index;

    public void OnCreated(ThinkerParams thinkerParams)
    {
        this.delay = thinkerParams.Delay;
        this.blastRadius = thinkerParams.BlastRadius;
        this.damage = thinkerParams.Damage;
        this.index = thinkerParams.Index;
        Debug.Log($"modifier for bomb explode created with thinkerParams {thinkerParams}");
        Invoke(nameof(OnIntervalThink), delay);
    }

    public void OnIntervalThink()
    {
        // find hit enemies 
        List<Client> hitTargets = FindTargetInBlastRadius(index, blastRadius);

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

        // sfx
    }

    private List<Client> FindTargetInBlastRadius(Vector2Int cursorIndex, int maxRadius)
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
