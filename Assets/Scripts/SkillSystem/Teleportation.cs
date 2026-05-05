using System.Collections.Generic;
using UnityEngine;

// implementation of teleportation effect
public class Teleportation : MySkill
{
    //---------------------------PUBLIC METHOD-----------------------------------
    public override void OnSkillStart()
    {
        if (GameManager.GetInstance().GetEntitiesDatabase().CountTargettedEntities() > 1)
        {
            Client target = GameManager.GetInstance().GetEntitiesDatabase   ().GetTargettedEntities()[0];
            Client destination = GameManager.GetInstance().GetEntitiesDatabase().GetTargettedEntities()[1];
            
            if (target != destination && !target.WalkableTile && destination.WalkableTile)
            {
                target.GameObject.transform.position = destination.Position;
            }

            GameManager.GetInstance().GetEntitiesDatabase().ClearTargettedEntities();
        }
    }

    public override bool OnSkillPhaseStart()
    {
        return true;
    }

    public override void OnUpgrade()
    {
    }
}