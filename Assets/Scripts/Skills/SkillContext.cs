using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents the data required to execute a skill.
/// Contains the selected targets and an optional position,
/// acting as a bridge between input collection and skill execution.
/// </summary>
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