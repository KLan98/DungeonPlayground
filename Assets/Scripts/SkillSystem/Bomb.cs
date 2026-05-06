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

        PrimaryKey key = new PrimaryKey() {SkillID = SkillID.BOMB, Level = 1};

        MyAPI.CreateThinker(key, SkillCursorController.Instance.transform.position);

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