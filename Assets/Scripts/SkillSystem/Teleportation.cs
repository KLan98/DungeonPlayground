using System.Collections.Generic;
using UnityEngine;

// implementation of teleportation effect
public class Teleportation : MySkill
{
    private ITargetable target;
    private ITargetable destination;
    private bool hasValidTeleTarget;
    private bool hasValidTelePosition;
    private GameObjectTarget teleTarget;
    private Vector2 telePosition;

    public Teleportation()
    {
        skillID = SkillID.WIND_TELEPORTATION;
        damage = new DynamicStats(StatName.DAMAGE, 0);
        cost = new DynamicStats(StatName.COST, 1);
        level = new DynamicStats(StatName.LEVEL, 1);
    }

    //---------------------------PUBLIC METHOD-----------------------------------
    public override void OnSkillStart()
    {
        base.OnSkillStart();

        if (target == null)
        {
            target = SkillCursorController.Instance.GetCursorTarget();

            if (target is TileTarget tile)
            {
                Debug.Log($"{tile} is not a valid target");
            }

            else if (target is GameObjectTarget gameObject)
            {
                hasValidTeleTarget = true;

                teleTarget = gameObject;

                Debug.Log($"{teleTarget.GameObject.name} as target for {this}");

                return;
            }

            else
            {
                Debug.LogWarning($"{target} is not a valid target for {this}");
            }
        }

        if (hasValidTeleTarget)
        {
            destination = SkillCursorController.Instance.GetCursorTarget();

            if (destination is GameObjectTarget gameObject)
            {
                Debug.Log($"{gameObject.GameObject.name} is not a valid target");
                return;
            }

            else if (destination is TileTarget tile)
            {
                telePosition = tile.CellPosition;

                hasValidTelePosition = true;

                Debug.Log($"Destination {telePosition} has been chosen for {teleTarget}");
            }

            else
            {
                Debug.LogWarning($"{destination} is not a valid target for {this}");
            }
        }

        // some delay 

        if (hasValidTelePosition && hasValidTeleTarget)
        {
            teleTarget.GameObject.transform.position = telePosition;

            OnSkillPhaseInterrupted();

            Debug.Log($"{teleTarget.GameObject.name} teleported to destinatiion {telePosition}");
        }

        // vfx

    }

    public override bool OnSkillPhaseStart()
    {
        base.OnSkillPhaseStart();

        return true;
    }

    public override void OnUpgrade()
    {
        base.OnUpgrade();
    }

    public override void OnSkillPhaseInterrupted()
    {
        base.OnSkillPhaseInterrupted();
        destination = null;
        target = null;
        teleTarget = null;
        hasValidTeleTarget = false;
        hasValidTelePosition = false;
        telePosition = Vector2.zero;
    }
}