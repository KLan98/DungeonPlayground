using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EffectType
{
    EFFECT_TYPE_DEAL_DAMAGE_SINGLE_TURN, 
    EFFECT_TYPE_BUFF, // de-hex, buff damage, buff health
    EFFECT_TYPE_DEBUFF, // hex, reduce armor, reduce health
    EFFECT_TYPE_SUPPORT, // teleportation, invisibility
    EFFECT_TYPE_HEAL, 
    EFFECT_TYPE_DAMAGE_PER_TURN, // poison, 
    EFFECT_TYPE_LINK, // link multiple targets 
}
