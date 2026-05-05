using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MySkill
{
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

        // LAN_TODO: where should skill takes their source of truth? Synchronization problem may occurs
        byte damage = GameManager.GetInstance().GetSkillsDatabase().GetOffensiveSkill(0).Damage;
        byte blastRadius = GameManager.GetInstance().GetSkillsDatabase().GetCrowdControlSkill(0).BlastRadius;

        // LAN_TODO: currently hardcoding values for thinker param, where is the source of truth regarding the skill's current level stored?
        ThinkerParams thinkerParams = new ThinkerParams { Damage = damage, Delay = 0.5f, BlastRadius = blastRadius, Index = SkillCursorController.Instance.GetCursorIndex(), Level = 1 };
        MyAPI.CreateThinker(SkillID.BOMB, SkillCursorController.Instance.transform.position, thinkerParams);
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
    }
}