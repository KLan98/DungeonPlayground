using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Modifiers : MonoBehaviour
{
    public virtual void OnCreated()
    {
    }

    public virtual void OnCreated(PrimaryKey primaryKey)
    {
    }

    protected virtual void OnThinkOnce()
    {
    }

    protected virtual void OnIntervalThink()
    {
    }

    protected virtual List<Client> FindTargetsInBlastRadius(Vector2Int cursorIndex, int maxRadius)
    {
        return null;
    }

    /// <summary>
    /// Start this modifier's think function (OnIntervalThink) with the given interval (float). To stop, call with -1.  
    /// </summary>
    /// <param name="interval"></param>
    protected virtual void StartIntervalThink(float interval)
    {
    }

    /// <summary>
    /// Runs when the modifier is destroyed (before unit loses modifier). 
    /// </summary>
    protected virtual void OnRemoved()
    {
    }
}
