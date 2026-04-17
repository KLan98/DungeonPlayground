#nullable enable
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
    private readonly List<GameObject>? targets;
    public IReadOnlyList<GameObject>? Targets
    {
        get { return targets; }
    }

    private readonly Vector2Int? castIndex;
    public Vector2Int? CastIndex
    {
        get { return castIndex; }
    }

    private readonly Vector2? newPosition;
    public Vector2? NewPosition
    {
        get { return newPosition; }
    }

    public SkillContext(List<GameObject>? targets = null, Vector2Int? castIndex = null, Vector2? newPosition = null)
    {
        this.targets = targets;
        this.castIndex = castIndex;
        this.newPosition = newPosition;
    }
}