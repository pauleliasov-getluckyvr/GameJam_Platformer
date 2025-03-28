using UnityEngine;

/// <summary>
/// Base class for all level objects
/// </summary>
[System.Serializable]
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
[System.Serializable]
public class PlatformData : LevelObject
{
    public bool isMoving;
    public bool isDestructible;
    public float speed;
    public float waitTime;
    public string movementType;
}

/// <summary>
/// Collectible data for level serialization
/// </summary>
[System.Serializable]
public class CollectibleData : LevelObject
{
    public string collectibleType;
    public float value;
}

/// <summary>
/// Obstacle data for level serialization
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