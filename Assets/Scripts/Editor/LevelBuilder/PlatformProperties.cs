using UnityEngine;

/// <summary>
/// Editor data structure for platform configuration
/// </summary>
[System.Serializable]
public class PlatformProperties
{
    public bool isMoving = false;
    public string movementType = "horizontal";
    public float speed = 2f;
    public float distance = 2f;
    public bool isDestructible = false;
    public float health = 100f;
} 