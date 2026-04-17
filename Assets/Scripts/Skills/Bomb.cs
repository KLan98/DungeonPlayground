using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : Skill
{
    private List<Vector2Int> impactRange;

    public Bomb(SkillData data) : base(data)
    {
        impactRange = new List<Vector2Int>();
    }

    public override void CastSkill(SkillContext context)
    {
        // perform place bomb, compute the impact range, trigger bomb explode
        Vector2Int castIndex = (Vector2Int)context.CastIndex;

        PerformPlaceBomb(castIndex);
        ComputeImpactRange(castIndex, impactRange);    
        if (impactRange.Count > 0)
        {
            BombTriggerExplode();
        }
    }

    /// <summary>
    /// Places a bomb at the specified grid location.
    /// </summary>
    /// <param name="destinationIndex">The grid coordinates where the bomb will be placed.</param>
    private void PerformPlaceBomb(Vector2Int destinationIndex)
    {
        // spawn bomb
        // trigger animation
    }

    private void BombTriggerExplode()
    {
        Debug.Log("Bomb exploded");
    }

    // BALANCE_ISSUE should the impact range be hardcoded?
    private void ComputeImpactRange(Vector2Int destinationIndex, List<Vector2Int> impactRange)
    {
        for (int x = -2; x <= 2; x++)
        {
            for (int y = -2; y <= 2; y++)
            {
                int manhattanDistance = Mathf.Abs(x) + Mathf.Abs(y);
                bool isInDiamondRing = manhattanDistance == 1 || manhattanDistance == 2;

                if (isInDiamondRing)
                {
                    impactRange.Add(destinationIndex + new Vector2Int(x, y));
                }
            }
        }
    }
}