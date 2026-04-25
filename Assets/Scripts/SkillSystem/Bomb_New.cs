using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb_New : MySkill
{
    public Bomb_New()
    {
        skillID = SkillID.BOMB;
        damage = new DynamicStats(StatName.DAMAGE, 10);
        cost = new DynamicStats(StatName.COST, 1);
        blastRadius = new DynamicStats(StatName.BLAST_RADIUS, 2);
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
        ThinkerParams thinkerParams = new ThinkerParams { Damage = this.damage.Value, Delay = 0.5f, BlastRadius = this.blastRadius.Value, Index = SkillCursorController.Instance.GetCursorIndex()};
        MyAPI.CreateThinker(skillID, Vector2.one, thinkerParams);
        Debug.Log($"thinkerParams {thinkerParams.Damage}, {thinkerParams.Delay}, {thinkerParams.Index}, {thinkerParams.BlastRadius}");
    }

    public override bool OnSkillPhaseStart()
    {
        base.OnSkillPhaseStart();
        return true;
    }

    public override void OnSkillPhaseInterrupted()
    {
        base.OnSkillPhaseInterrupted();
        // skill interrupted
        Debug.Log($"{this} is interrupted");
    }
}