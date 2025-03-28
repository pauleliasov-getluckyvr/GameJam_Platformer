using UnityEngine;

/// <summary>
/// Editor component for handling moving platform behavior in the level builder
/// </summary>
public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private float speed = 2f;
    [SerializeField] private float waitTime = 0.5f;
    [SerializeField] private bool isMoving = true;
    [SerializeField] private string movementType = "horizontal";
    [SerializeField] private float distance = 2f;

    private Vector2 startPoint;
    private Vector2 endPoint;

    public void Initialize(Vector2 start, PlatformProperties properties)
    {
        transform.position = start;
        startPoint = start;
        
        speed = properties.speed;
        isMoving = properties.isMoving;
        movementType = properties.movementType;
        distance = properties.distance;
        
        endPoint = CalculateEndPoint();
    }

    private Vector2 CalculateEndPoint()
    {
        if (movementType == "horizontal")
        {
            return startPoint + new Vector2(distance, 0);
        }
        else
        {
            return startPoint + new Vector2(0, distance);
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (!isMoving) return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(startPoint, CalculateEndPoint());
        Gizmos.DrawWireSphere(startPoint, 0.2f);
        Gizmos.DrawWireSphere(CalculateEndPoint(), 0.2f);
    }
#endif
} 