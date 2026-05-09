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
        EntitiesManager.GetInstance().AddTargettedEntity(SkillCursorController.Instance.NearByClients[0]);

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

        // create primary key for testing purpose
        PrimaryKey primaryKey = new PrimaryKey() { SkillID = SkillID.BOMB, Level = 1 };
        
        byte cost = GameManager.GetInstance().GetSkillsDatabase().GetCostTable(1).Cost;

        CrowdControlSkill cc = GameManager.GetInstance().GetSkillsDatabase().GetCrowdControlSkill(primaryKey);

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

        byte cost = GameManager.GetInstance().GetSkillsDatabase().GetCostTable(1).Cost;

        if (!SkillCursorController.Instance.gameObject.activeInHierarchy)
        {
            SkillCursorController.Instance.ToggleActive();
        }
    }
}