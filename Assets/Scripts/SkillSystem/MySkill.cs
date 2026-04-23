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

    //---------------------------------SKILL EVENTS---------------------------------

    /// <summary>
    /// When the skill is leveled up. No return type, no parameters
    /// </summary>
    public virtual void OnUpgrade()
    {

    }

    /// <summary>
    /// When player has confirmed the casting of this skill, most skills begin to do their work in this function. No return type, no parameters
    /// </summary>
    public virtual void OnSkillStart()
    {

    }

    /// <summary>
    /// The first event fired after player uses an ability. Return true if player has enough resource to cast this skill, return false if not. No return type, no parameters
    /// </summary>
    /// <returns></returns>
    public virtual bool OnSkillPhaseStart()
    {

        return true;   
    }
}
