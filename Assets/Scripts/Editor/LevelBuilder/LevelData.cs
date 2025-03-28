using UnityEngine;
using System;

/// <summary>
/// Data structure for level configuration
/// </summary>
[Serializable]
public class LevelData
{
    public string levelName;
    public Vector2 startPoint;
    public Vector2 endPoint;
    public LevelObject[] objects;
}

/// <summary>
/// Base class for all level objects
/// </summary>
[Serializable]
public class LevelObject
{
    public string type;
    public Vector2 position;
    public Vector2 scale = Vector2.one;
    public float rotation;
}

/// <summary>
/// Platform data for level serialization
/// </summary>
[Serializable]
public class PlatformData : LevelObject
{
    public bool isMoving;
    public bool isDestructible;
    public float speed;
    public float waitTime;
    public Vector2[] waypoints;
}

/// <summary>
/// Collectible data for level serialization
/// </summary>
[Serializable]
public class CollectibleData : LevelObject
{
    public string collectibleType;
    public float value;
}

/// <summary>
/// Obstacle data for level serialization
/// </summary>
[Serializable]
public class ObstacleData : LevelObject
{
    public string obstacleType;
    public float damage;
    public bool isDestructible;
    public float health;
} 