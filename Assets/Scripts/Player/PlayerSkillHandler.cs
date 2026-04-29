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
    [SerializeField] private SkillCursorController skillCursor;
    [SerializeField] private GameManager gameManager;
    private MySkill activeSkill;
    private GameObject blastRadius;

    //---------------------BUILT-IN METHODS------------------------------

    private void Awake()
    {
    }

    //----------------------EVENT RESPONSE--------------------------------
    public void OnCursorConfirm()
    {
        ITargetable target = SkillCursorController.Instance.GetCursorTarget();

        if (target is null)
        {
            return;
        }

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
        OnSkillInterrupted();

        activeSkill = gameManager.Bomb;

        if (activeSkill == null || !activeSkill.OnSkillPhaseStart())
        {
            return;
        }

        if (!SkillCursorController.Instance.gameObject.activeInHierarchy)
        {
            SkillCursorController.Instance.ToggleActive();
        }

        // pass blast radius of active skill to skill cursor controller
        blastRadius = SkillCursorController.Instance.SpawnBlastRadiusTiles(activeSkill.BlastRadius.Value);
    }

    public void CastTeleportation()
    {
        OnSkillInterrupted();

        activeSkill = gameManager.Teleportation;

        if (activeSkill == null || !activeSkill.OnSkillPhaseStart())
        {
            return;
        }

        if (!SkillCursorController.Instance.gameObject.activeInHierarchy)
        {
            SkillCursorController.Instance.ToggleActive();
        }
    }
}