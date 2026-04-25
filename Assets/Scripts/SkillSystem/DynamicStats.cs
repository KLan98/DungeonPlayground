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

    public void SetValue(int newValue)
    {
        if (newValue != value)
        {
            value = newValue;
        }
    }

    public void AdjustValue(int amount)
    {
        SetValue(value + amount);
    }

    public void ResetToMax()
    {
        SetValue(maxValue);
    }

    public void ResetToMin()
    {
        SetValue(minValue);
    }

    // event system when this stat changes the other code entities/systems should react accordingly

}

public enum StatName
{
    DAMAGE,
    COST,
    HEALTH,
    BLAST_RADIUS,
}