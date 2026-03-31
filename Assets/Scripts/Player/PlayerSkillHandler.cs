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
    
    private Skill pendingSkill;
    
    [SerializeField] private List<SkillDefinition> skillDefinitions; // skills player can cast
    [SerializeField] private List<GameObject> cachedTargets;

    private SkillLoadout skillLoadout;

    private void Awake()
    {
        skillLoadout = new SkillLoadout(skillDefinitions);
        cachedTargets = new List<GameObject>();
    }

    public void SelectSkill(int index)
    {
        pendingSkill = skillLoadout.GetSkill(index);
        Debug.Log($"Pending skill is {pendingSkill.Data.skillID}");

        // Tell cursor exactly what to collect
        skillCursor.BeginSelection(pendingSkill.SkillRequirements);
    }

    // response of Vector2 event
    public void OnPositionConfirmed(Vector2 position)
    {
        if (cachedTargets != null && cachedTargets.Count > 0)
        {
            var context = new SkillContext(cachedTargets, position);
            pendingSkill.CastSkill(context);
            pendingSkill = null;

            skillCursor.IsActive = false;

            ResetTargets(); 
        }
    }

    // response for skills that do not need position 
    public void OnTargetsConfirmed()
    {
        // SkillContext is built HERE — it's the handoff point between
        // "what the cursor collected" and "what the skill needs"
        if (cachedTargets != null && cachedTargets.Count > 0)
        {
            var context = new SkillContext(cachedTargets, null);

            pendingSkill.CastSkill(context);
            pendingSkill = null;

            skillCursor.IsActive = false;
        
            ResetTargets();
        }
    }

    // cache target on skill
    public void OnCacheTarget(GameObject target)
    {
        cachedTargets.Add(target);
    }

    // remove cached target
    public void OnRemoveCachedTarget(GameObject target)
    {
        if (cachedTargets.Contains(target))
        {
            cachedTargets.Remove(target);
        }
    }

    // LAN_TODO: event channel
    public void SelectTeleport()
    {
        SelectSkill(0);
    }

    private void ResetTargets()
    {
        // clear cached targets
        cachedTargets.Clear();
    }
}