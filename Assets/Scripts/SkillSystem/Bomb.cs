using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MySkill
{
    public Bomb()
    {
        skillID = SkillID.BOMB;
        damage = new DynamicStats(StatName.DAMAGE, 10);
        cost = new DynamicStats(StatName.COST, 1);
        level = new DynamicStats(StatName.LEVEL, 2);

        // only valid for first time instantiate
        switch (level.Value)
        {
            case 1:
                blastRadius = new DynamicStats(StatName.BLAST_RADIUS, 1);
                break;
            case 2:
                blastRadius = new DynamicStats(StatName.BLAST_RADIUS, 2);
                break;
        }
        //Icon = SkillAssets.GetIcon(skillID);
    }

    //---------------------------PUBLIC METHOD-----------------------------------
    public override void OnSkillStart()
    {
        base.OnSkillStart();
        // Filter out the game object target
        ITargetable target = SkillCursorController.Instance.GetCursorTarget();

        if (target is GameObjectTarget gameObject)
        {
            Debug.Log($"{gameObject.GameObject.name} is not a valid target");
            return;
        }
        
        // spawn thinker at position, an invisible, invulnerable unit, that handles
        // finding nearby enemies dealing damage to enemies
        // the targetfx needs to know about the effect file, as well as the origin for soundfx
        ThinkerParams thinkerParams = new ThinkerParams { Damage = damage.Value, Delay = 0.5f, BlastRadius = blastRadius.Value, Index = SkillCursorController.Instance.GetCursorIndex(), Level = level.Value};
        MyAPI.CreateThinker(skillID, SkillCursorController.Instance.transform.position, thinkerParams);
        //Debug.Log($"thinkerParams {thinkerParams.Damage}, {thinkerParams.Delay}, {thinkerParams.Index}, {thinkerParams.BlastRadius}");

        if (SkillCursorController.Instance.gameObject.activeInHierarchy)
        {
            SkillCursorController.Instance.ToggleActive();
        }
    }

    public override bool OnSkillPhaseStart()
    {
        base.OnSkillPhaseStart();
        return true;
    }

    public override void OnSkillPhaseInterrupted()
    {
        base.OnSkillPhaseInterrupted();
    }

    public override void OnUpgrade()
    {
        // fire event for updating level, text, sprite, blast radius, damage...
    }
}