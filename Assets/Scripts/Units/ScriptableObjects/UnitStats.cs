using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Stat", menuName = "Units/UnitStats")]
public class UnitStats : ScriptableObject
{
    [SerializeField] private int strength;
    public int Strength
    {
        get { return strength; }
        set
        {
            if (strength <= 10 && strength > 0)
            {
                strength = value;
            }
        }
    }

    [SerializeField] private int endurance;
    public int Endurance
    {
        get { return endurance; }
        set 
        { 
            if (endurance <= 10 && endurance > 0)
            { 
                endurance = value;
            }
        }
    }

    [SerializeField] private int agility;
    public int Agility
    {
        get { return agility; }
        set
        {
            if (agility <= 10 && agility > 0 )
            {
                agility = value;
            }
        }
    }
    
    [SerializeField] private int luck;
    public int Luck
    {
        get { return luck; }
        set
        {
            if (luck <= 10 && luck > 0)
            {
                luck = value;
            }
        }
    }
}
