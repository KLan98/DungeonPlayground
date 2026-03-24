using UnityEngine;

public class TeleportationEffect : ISkillEffect
{
    private int turnsLeftUntilReuse;

    public TeleportationEffect()
    {
        turnsLeftUntilReuse = 0;
    }

    public void Apply(GameObject target)
    {
        if (turnsLeftUntilReuse == 0)
        {
            //targets[0].transform.position = newPosition;
        }

        else
        { 
        }
    }
}
