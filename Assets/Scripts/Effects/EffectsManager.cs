using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectsManager
{
    // output the effect 
    public static GameObject CreateEffect(GameObject effectPrefab, EffectAttach effectAttach, GameObject owningEntity)
    {
        if (effectPrefab == null)
        {
            Debug.LogWarning("Effect prefab = null");
            return null;
        }

        GameObject effectInstance = null;

        // handler for EffectAttach
        switch (effectAttach)
        {
            case EffectAttach.ATTACH_WORLD:
                effectInstance = Object.Instantiate(effectPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                break;
            case EffectAttach.ATTACH_CUSTOMORIGIN:
                if (owningEntity == null)
                {
                    Debug.LogWarning($"{effectAttach} needs an owningEntity");
                    break;
                }
                break;
            
        }

        return effectInstance;
    }

    // input effect and change its properties
    public static void SetEffectControl(GameObject effectInstance, Vector2? position = null, Quaternion? rotation = null, Color? color = null, float? size = null, GameObject? owningEntity = null)
    {
        if (effectInstance == null)
        {
            Debug.LogWarning("Effect instance is null");
            return;
        }
        
        if (position.HasValue)
        {
            effectInstance.transform.position = position.Value;
        }

        if (rotation.HasValue)
        {

        }

        if (color.HasValue)
        {

        }

        if (size.HasValue)
        {

        }

        if (owningEntity != null)
        {

        }
    }
}

/// <summary>
/// The type of attachment for the effect
/// </summary>
public enum EffectAttach
{
    ATTACH_WORLD, // spawn at world position, not attached to anything
    ATTACH_CUSTOMORIGIN, // spawn at custom origin, custom attached
    //PATTACH_ABSORIGIN 	0 	Spawn on entity origin.
    //PATTACH_ABSORIGIN_FOLLOW 	1 	Follow the entity origin.
    ATTACH_CUSTOMORIGIN_FOLLOW, 	
    ATTACH_POINT,
}