using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : Skill
{
    private List<Vector2Int> impactPattern;

    public Bomb(SkillData data) : base(data)
    {
        impactPattern = new List<Vector2Int>();
    }

    public override void CastSkill(SkillContext context)
    {
        // perform place bomb, compute the impact range, trigger bomb explode
        Vector2Int castIndex = (Vector2Int)context.CastIndex;

        PerformPlaceBomb(castIndex);

        // recompute the impact range since the cast index might changed
        impactPattern.Clear();
        ImpactPattern(castIndex, impactPattern);
        
        if (impactPattern.Count > 0)
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

    // wait until bomb animation is finished then explode 
    private void BombTriggerExplode()
    {
        foreach (var item in impactPattern)
        {
            Debug.Log("Bomb exploded at " + item);
        }
    }

    // impact pattern for bomb is a diamond, this is only the definition
    private void ImpactPattern(Vector2Int cursorIndex, List<Vector2Int> impactRange)
    {
        for (int x = -2; x <= 2; x++)
        {
            for (int y = -2; y <= 2; y++)
            {
                int manhattanDistance = Mathf.Abs(x) + Mathf.Abs(y);
                bool isInDiamondRing = manhattanDistance == 1 || manhattanDistance == 2;

                if (isInDiamondRing)
                {
                    impactRange.Add(cursorIndex + new Vector2Int(x, y));
                }
            }
        }
    }

    public override List<Vector2Int> GetImpactPattern(Vector2Int cursorIndex)
    {
        ImpactPattern(cursorIndex, impactPattern);
        return impactPattern;
    }
}