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
    public SkillCursorController skillCursor;
    
    static private Skill pendingSkill;
    
    [SerializeField] private List<SkillDefinition> skillDefinitions; // skills player can cast

    private SkillLoadout skillLoadout;

    private void Awake()
    {
        skillLoadout = new SkillLoadout(skillDefinitions);

        // implement event system
        //skillCursor.OnTargetsConfirmed = OnTargetsConfirmed;
    }

    public void SelectSkill(int index)
    {
        pendingSkill = skillLoadout.GetSkill(index);
        Debug.Log($"Pending skill is {pendingSkill.Data.skillID}");

        // Tell cursor exactly what to collect
        skillCursor.BeginSelection(pendingSkill.SkillRequirements);
    }

    public static void OnTargetsConfirmed(List<GameObject> targets, Vector2? position)
    {
        // SkillContext is built HERE — it's the handoff point between
        // "what the cursor collected" and "what the skill needs"
        var context = new SkillContext(targets, position);
        
        pendingSkill.CastSkill(context);

        pendingSkill = null;
        //skillCursor.IsActive = false;
    }

    public void SelectTeleport()
    {
        SelectSkill(0);
    }

    public void SelectLightningChain()
    {
        SelectSkill(1);
    }
}