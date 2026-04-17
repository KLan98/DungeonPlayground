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

    [SerializeField] private Vector2Int cachedIndex;

    private SkillLoadout skillLoadout;

    //---------------------BUILT-IN METHODS------------------------------

    private void Awake()
    {
        skillLoadout = new SkillLoadout(skillDefinitions);
        cachedTargets = new List<GameObject>();
    }

    //----------------------EVENT RESPONSE--------------------------------

    /// <summary>
    /// Handles the confirmation of a position selection and initiates the skill casting process at the specified position.
    /// </summary>
    /// <remarks>This method is called in response to a user action confirming a target position for a skill. It processes any cached targets and deactivates the skill cursor after casting.</remarks>
    /// <param name="position">The position, in world coordinates, where the skill should be cast.</param>
    public void OnPositionConfirmed(Vector2 position)
    {
        if (cachedTargets != null && cachedTargets.Count > 0)
        {
            var context = new SkillContext(cachedTargets, null, position);
            pendingSkill.CastSkill(context);
            pendingSkill = null;

            skillCursor.IsActive = false;

            ResetCachedTargets(); 
        }
    }

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
        
            ResetCachedTargets();
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

    // LAN_TODO should this be used? Use position confirmed is also possible, since placing a game object at the same position as the cell tile is also not bad
    /// <summary>
    /// Some skills do not have a game object as target, for example bomb where the bomb is placed in an empty tile 
    /// </summary>
    /// <param name="destinationIndex"></param>
    public void OnDestinationIndexConfirmed(Vector2Int destinationIndex)
    {
        if (cachedIndex.x != int.MaxValue && cachedIndex.y != int.MaxValue)
        {
            var context = new SkillContext(null, cachedIndex);

            pendingSkill.CastSkill(context);
            pendingSkill = null;

            ResetCachedIndices();
        }
    }

    //--------------------------PRIVATE METHODS------------------------
    private void SelectSkill(int index)
    {
        pendingSkill = skillLoadout.GetSkill(index);
        Debug.Log($"Pending skill is {pendingSkill.SkillData.SkillID}");

        // Tell cursor exactly what to collect
        skillCursor.BeginSelection(pendingSkill.SkillRequirements);
    }

    private void ResetCachedTargets()
    {
        // clear cached targets
        cachedTargets.Clear();
    }

    private void ResetCachedIndices()
    {
        cachedIndex = new Vector2Int(int.MaxValue, int.MaxValue);
    }
    
    //-----------------------ON-CLICK RESPONSE------------------------------
    // LAN_TODO: event channel, the current implementation is hard-coded
    public void SelectTeleport()
    {
        SelectSkill(0);
    }

    public void SelectBomb()
    {
        SelectSkill(1);
    }
}