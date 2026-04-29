using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicStats
{
    private StatName name;
    public StatName Name
    {
        get
        {
            return name;
        }
    }

    private int value;
    public int Value
    {
        get
        {
            return value;
        }
    }

    private int maxValue;
    public int MaxValue
    {
        get
        {
            return maxValue;
        }
        set
        {
            maxValue = value;
        }
    }

    private int minValue;
    public int MinValue
    {
        get
        {
            return minValue;
        }
        set
        {
            minValue = value;
        }
    }

    public DynamicStats(StatName name, int value)
    {
        this.name = name;
        this.value = value;
        this.maxValue = value;
        this.minValue = 0;
    }

    //--------------------------------PUBLIC METHODS------------------------------------
    /// <summary>
    /// Buff or debuff stat
    /// </summary>
    /// <param name="amount"></param>
    /// amount can be > 0 or < 0
    public void AdjustValue(int amount)
    {
        if (amount == 0)
        {
            Debug.LogWarning("Parameter cannot be 0");
        }

        int result = value + amount;
        Mathf.Clamp(value, minValue, maxValue);
        SetValue(result);
    }

    public void ResetToMax()
    {
        SetValue(maxValue);
    }

    public void ResetToMin()
    {
        SetValue(minValue);
    }

    //-----------------------------EVENT--------------------------------------------
    // LAN_TODO event system when this stat changes the other code entities/systems should react accordingly
    public void OnStatChanged()
    {

    }


    //--------------------------PRIVATE METHODS------------------------------------
    private void SetValue(int newValue)
    {
        value = newValue;
    }
}

public enum StatName
{
    DAMAGE,
    COST,
    HEALTH,
    BLAST_RADIUS,
    LEVEL,
}