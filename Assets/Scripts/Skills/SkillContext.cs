using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillContext
{
    public IReadOnlyList<GameObject> Targets { get; }

    public Vector2? NewPosition { get; }

    public SkillContext(List<GameObject> targets, Vector2? newPosition = null)
    {
        Targets = targets;
        NewPosition = newPosition;
    }
}