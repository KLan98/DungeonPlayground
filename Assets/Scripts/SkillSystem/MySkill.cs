using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MySkill
{
    //---------------------------------SKILL EVENTS---------------------------------

    /// <summary>
    /// When the skill is leveled up. No return type, no parameters
    /// </summary>
    public virtual void OnUpgrade()
    {
        Debug.Log($"OnUpgrade of {this} is called");
    }

    /// <summary>
    /// When player has confirmed the casting of this skill, most skills begin to do their work in this function. No return type, no parameters
    /// </summary>
    public virtual void OnSkillStart()
    {
        Debug.Log($"OnSkillStart of {this} is called");
    }

    /// <summary>
    /// The first event fired after player uses an ability. Return true if player has enough resource to cast this skill, return false if not. No return type, no parameters
    /// </summary>
    /// <returns></returns>
    public virtual bool OnSkillPhaseStart()
    {
        Debug.Log($"OnSkillPhaseStart of {this} is called");

        // LAN_TODO: add checks for player's resources

        return true;
    }

    /// <summary>
    /// When skill phase is cancelled for any reason, reset all private fields of specific skill to init states. No return type, no parameters
    /// </summary>
    public virtual void OnSkillPhaseInterrupted()
    {
        Debug.Log($"OnSkillPhaseInterrupted of {this} is called");

        if (SkillCursorController.Instance.gameObject.activeInHierarchy)
        {
            SkillCursorController.Instance.ToggleActive();
        }
    }
}
