using UnityEngine;

/// <summary>
/// Base class for all level objects
/// </summary>
[System.Serializable]
public class LevelObject
{
    public string type;
    public Vector2 position;
    public Vector2 scale;
    public float rotation;
}

/// <summary>
/// Data structure for platform objects
/// </summary>
[System.Serializable]
public class PlatformData : LevelObject
{
    public bool isMoving;
    public bool isDestructible;
    public string movementType;
    public float speed;
    public float distance;
    public float health;
}

/// <summary>
/// Data structure for collectible objects
/// </summary>
[System.Serializable]
public class CollectibleData : LevelObject
{
    public string collectibleType;
    public float value;
}

/// <summary>
/// Data structure for obstacle objects
/// </summary>
[System.Serializable]
public class ObstacleData : LevelObject
{
    public string obstacleType;
    public float damage;
    public bool isDestructible;
    public float health;
}

/// <summary>
/// Data structure for level configuration
/// </summary>
[System.Serializable]
public class LevelData
{
    public string levelName;
    public Vector2 startPoint;
    public Vector2 endPoint;
    public LevelObject[] objects;
} 