using System.Collections.Generic;
using UnityEngine;

// implementation of teleportation effect
public class Teleportation : Skill
{
    public Teleportation(SkillData data) : base(data) { }

    public override void CastSkill(SkillContext context)
    {
        GameObject target = context.Targets[0];
        Vector2 newPosition = (Vector2)context.NewPosition;

        PerformTeleportation(target, newPosition);
    }

    private void PerformTeleportation(GameObject target, Vector2 newPosition)
    {
        target.transform.position = newPosition;
    }
}