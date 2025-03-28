using UnityEngine;

/// <summary>
/// Marker component for obstacle objects
/// </summary>
public class ObstacleComponent : MonoBehaviour
{
    public string obstacleType;
    public float damage = 10f;
    public bool isDestructible;
    public float health = 100f;
} 