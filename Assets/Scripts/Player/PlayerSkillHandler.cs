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

    private MySkill activeSkill;

    [SerializeField] private GameManager gameManager;

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
            SkillCursorController.Instance.gameObject.SetActive(false);
        }
    }

    //--------------------------PRIVATE METHODS------------------------

    //-----------------------ON-CLICK RESPONSE------------------------------

    // LAN_TODO: whenever a skill is selected it calls the cast skill method
    // this method handles the enabling of cursor 
    public void CastBomb()
    {
        activeSkill = gameManager.Bomb;

        if (activeSkill == null || !activeSkill.OnSkillPhaseStart())
        {
            return;
        }

        SkillCursorController.Instance.gameObject.SetActive(true);

        // pass blast radius of active skill to skill cursor controller
        SkillCursorController.Instance.SpawnBlastRadiusTiles(activeSkill.BlastRadius.Value);
    }
}