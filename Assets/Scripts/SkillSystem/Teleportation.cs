using System.Collections.Generic;
using UnityEngine;

// implementation of teleportation effect
public class Teleportation : MySkill
{
    //---------------------------PUBLIC METHOD-----------------------------------
    public override void OnSkillStart()
    {
        if (EntitiesManager.GetInstance().CountTargettedEntities() > 1)
        {
            Client target = EntitiesManager.GetInstance().GetTargettedEntities()[0];
            Client destination = EntitiesManager.GetInstance().GetTargettedEntities()[1];
            
            if (target != destination && !target.WalkableTile && destination.WalkableTile)
            {
                target.GameObject.transform.position = destination.Position;
            }

            EntitiesManager.GetInstance().ClearTargettedEntities();
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