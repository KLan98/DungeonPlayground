using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModifierBombExplode : MonoBehaviour
{
    private float delay;
    private int maxRange;
    private int damage;
    private Vector2Int index;
    private DungeonGrid grid;

    public void OnCreated(ThinkerParams thinkerParams, DungeonGrid grid)
    {
        this.delay = thinkerParams.Delay;
        this.maxRange = thinkerParams.MaxRange;
        this.damage = thinkerParams.Damage;
        this.index = thinkerParams.Index;
        this.grid = grid;
        Debug.Log($"modifier for bomb explode created with thinkerParams {thinkerParams}, grid {grid} as arguments");
        Invoke(nameof(OnIntervalThink), delay);
    }

    public void OnIntervalThink()
    {
        // find hit enemies 
        List<Client> enemies = FindEnemiesInRadius(index, maxRange);

        // make each enemy takes damage
        foreach (Client enemy in enemies)
        {
            // can add elemental resistance
            if (enemy != null && !enemy.WalkableTile)
            {
                // construct damage table
                //DamageTable damageTable = {}
                DamageTable damageTable = new DamageTable();
                MyAPI.ApplyDamage(damageTable);
                Debug.Log($"{enemy} hit with bomb at {enemy.Position}");
            }
        }

        // vfx

        // sfx
    }

    private List<Client> FindEnemiesInRadius(Vector2Int cursorIndex, int maxRadius)
    {
        List<Client> enemies = new List<Client>();
        for (int x = -maxRadius; x <= maxRadius; x++)
        {
            for (int y = -maxRadius; y <= maxRadius; y++)
            {
                int manhattanDistance = Mathf.Abs(x) + Mathf.Abs(y);

                if (manhattanDistance > 0 && manhattanDistance <= maxRadius)
                {
                    Vector2Int index =new Vector2Int(x + cursorIndex.x, y + cursorIndex.y);
                    
                    Client client = grid.GetClientAtIndex(index);

                    if (client != null)
                    {
                        enemies.Add(client);    
                    }
                }
            }
        }

        return enemies;
    }

    private void ImpactPattern(Vector2Int cursorIndex, List<Vector2Int> impactPattern, int maxRadius)
    {
        for (int x = -maxRadius; x <= maxRadius; x++)
        {
            for (int y = -maxRadius; y <= maxRadius; y++)
            {
                int manhattanDistance = Mathf.Abs(x) + Mathf.Abs(y);
                bool isInDiamondRing = manhattanDistance == 1 || manhattanDistance == 2 || manhattanDistance == maxRadius;

                if (isInDiamondRing)
                {
                    impactPattern.Add(cursorIndex + new Vector2Int(x, y));
                }
            }
        }
    }
}
