using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct SkillElements
{
    public Element Element;
    public byte Level;
    public byte Cost;

    public SkillElements(Element element, byte level, byte cost)
    {
        Element = element;
        Level = level;
        Cost = cost;
    }
}

public enum Element
{
    Fire,
    Neutral,
    Ice,
    Electric,
}
