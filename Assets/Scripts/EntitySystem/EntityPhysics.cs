using System;
using UnityEngine;

[System.Serializable]
public struct EntityPhysics
{
    public Vector2 WorldPosition;
    public Vector2 Velocity;
    public Vector2Int MoveDirection;

    public EntityPhysics(Vector2 position, Vector2 velocity, Vector2Int moveDirection)
    {
        this.WorldPosition = position;
        this.Velocity = velocity;
        this.MoveDirection = moveDirection;
    }
}

public enum EntityType
{
    PLAYER,
    MINOTAUR,
    SLIME,
    BOT,
    BARBARIAN,
}