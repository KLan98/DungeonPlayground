using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles player skill selection and execution.
/// Initializes the player's SkillLoadout, allows selecting a skill by index,
/// and coordinates with the SkillCursorController to gather required targets.
/// Once targets are confirmed, it builds a SkillContext and casts the selected skill.
/// </summary>
public class PlayerSkillHandler : MonoBehaviour
{
    private MySkill activeSkill;
    private GameObject blastRadius;

    //---------------------BUILT-IN METHODS------------------------------

    private void Awake()
    {
    }

    //----------------------EVENT RESPONSE--------------------------------
    public void OnCursorConfirm()
    {
        GameManager.GetInstance().GetEntitiesDatabase().AddTargettedEntity(SkillCursorController.Instance.NearByClients[0]);

        if (activeSkill != null)
        {
            activeSkill.OnSkillStart();
        }
    }

    public void OnSkillInterrupted()
    {
        if (activeSkill != null)
        {
            activeSkill.OnSkillPhaseInterrupted();
            activeSkill = null;

            if (SkillCursorController.Instance.gameObject.activeInHierarchy)
            {
                SkillCursorController.Instance.ToggleActive();
            }

            if (blastRadius != null)
            {
                SkillCursorController.Instance.DestroyBlastRadius(blastRadius);
                blastRadius = null;
            }
        }
    }

    //-----------------------ON-CLICK RESPONSE------------------------------
    // LAN_NOTE: only place holder for testing cast bomb, will be deprecated
    public void CastBomb()
    {
        activeSkill = new Bomb();
        SkillElements element = GameManager.GetInstance().GetSkillsDatabase().GetSkill(0);

        byte cost = element.Cost; 
        
        CrowdControlSkill cc = GameManager.GetInstance().GetSkillsDatabase().GetCrowdControlSkill(0);

        // LAN_TODO, if cost > current turn cost then return null

        if (!SkillCursorController.Instance.gameObject.activeInHierarchy)
        {
            SkillCursorController.Instance.ToggleActive();
        }

        // pass blast radius of active skill to skill cursor controller
        GameObject blastRadius = SkillCursorController.Instance.SpawnBlastRadiusTiles(cc.BlastRadius);
    }

    public void CastTeleportation()
    {
        activeSkill = new Teleportation();

        byte cost = GameManager.GetInstance().GetSkillsDatabase().GetSkill(1).Cost;

        if (!SkillCursorController.Instance.gameObject.activeInHierarchy)
        {
            SkillCursorController.Instance.ToggleActive();
        }
    }
}