using UnityEngine;

/// <summary>
/// Marker component for platform objects
/// </summary>
public class PlatformComponent : MonoBehaviour
{
    public bool isMoving;
    public bool isDestructible;
    public float speed = 2f;
    public float waitTime = 1f;
    public string movementType = "Horizontal";
} 