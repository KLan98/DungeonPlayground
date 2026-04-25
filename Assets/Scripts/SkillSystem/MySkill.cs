using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MySkill
{
    //------------------------------------STAT DATA------------------------------
    protected SkillID skillID;
    public SkillID SkillID
    {
        get
        {
            return skillID;
        }
    }

    protected DynamicStats damage;
    public DynamicStats Damage
    {
        get
        {
            return damage;
        }
    }

    protected DynamicStats cost;
    public DynamicStats Cost
    {
        get
        {
            return cost;
        }
    }

    protected Sprite icon;
    public Sprite Icon
    {
        get
        {
            return icon;
        }
    }

    protected SkillTargetType skillTargetType;
    public SkillTargetType SkillTargetType
    {
        get
        {
            return skillTargetType;
        }
    }

    protected DynamicStats blastRadius;
    public DynamicStats BlastRadius
    {
        get
        {
            return blastRadius;
        }
    }

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
        return true;
    }

    /// <summary>
    /// When skill phase is cancelled for any reason. No return type, no parameters
    /// </summary>
    public virtual void OnSkillPhaseInterrupted()
    {
        Debug.Log($"OnSkillPhaseInterrupted of {this} is called");
    }
}
