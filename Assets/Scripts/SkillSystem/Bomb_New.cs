using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// this class contains everything about bomb skill, from its stat, to its effect, to sprite...
public class Bomb_New : MySkill
{
    private DungeonGrid grid;
    public Bomb_New(DungeonGrid grid)
    {
        skillID = SkillID.BOMB;
        damage = new DynamicStats(StatName.DAMAGE, 10);
        cost = new DynamicStats(StatName.COST, 1);
        this.grid = grid;
        //Icon = SkillAssets.GetIcon(skillID);
    }

    //---------------------------PUBLIC METHOD-----------------------------------
    public override void OnSkillStart()
    {
        ITargetable target = SkillCursorController.GetCursorTarget();

        if (target is GameObjectTarget gameObjectTarget || target is null)
        {
            return;
        }

        Vector2 position = SkillCursorController.GetCursorPosition();

        // spawn thinker at position, an invisible, invulnerable unit, that handles
        // finding nearby enemies dealing damage to enemies
        // the targetfx needs to know about the effect file, as well as the origin for soundfx
        ThinkerParams thinkerParams = new ThinkerParams { Damage = this.damage.Value, Delay = 0.5f, MaxRange = 2, Index = SkillCursorController.GetCursorIndex()};
        MyAPI.CreateThinker(skillID, Vector2.one, thinkerParams, grid);
        Debug.Log($"OnSkillStart called, with thinkerParams {thinkerParams.Damage}, {thinkerParams.Delay}, {thinkerParams.Index}");
    }

    public override bool OnSkillPhaseStart()
    {
        return true;
    }
}