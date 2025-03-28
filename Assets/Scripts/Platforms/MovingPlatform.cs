using UnityEngine;
using GameJamPlatformer.Core.Interfaces;

namespace GameJamPlatformer.Platforms
{
    /// <summary>
    /// Implementation of a moving platform that moves in a ping-pong pattern
    /// </summary>
    public class MovingPlatform : MonoBehaviour, IPlatform
    {
        private Vector2 _startPosition;
        private Vector2 _endPosition;
        private float _speed;
        private float _progress;
        private bool _movingForward = true;
        private PlatformProperties _properties;
        private BoxCollider2D _collider;

        public PlatformProperties Properties => _properties;

        private void Awake()
        {
            _collider = GetComponent<BoxCollider2D>();
            if (_collider == null)
            {
                _collider = gameObject.AddComponent<BoxCollider2D>();
            }
        }

        public void Initialize(PlatformProperties properties)
        {
            _properties = properties;
            _speed = properties.Speed;
            _startPosition = transform.position;

            // Calculate end position based on waypoints or default movement
            if (properties.WayPoints != null && properties.WayPoints.Length > 0)
            {
                _endPosition = properties.WayPoints[0];
            }
            else
            {
                // Default to right movement if no waypoints
                _endPosition = _startPosition + Vector2.right * 2f;
            }
        }

        public void UpdatePlatform()
        {
            if (!_properties.IsMoving) return;

            // Update progress
            float delta = Time.deltaTime * _speed;
            _progress += _movingForward ? delta : -delta;

            // Check for direction change
            if (_progress >= 1f)
            {
                _progress = 1f;
                _movingForward = false;
            }
            else if (_progress <= 0f)
            {
                _progress = 0f;
                _movingForward = true;
            }

            // Update position
            transform.position = Vector2.Lerp(_startPosition, _endPosition, _progress);
        }

        private void FixedUpdate()
        {
            UpdatePlatform();
        }

        public void OnPlayerLand(GameObject player)
        {
            if (player != null)
            {
                player.transform.SetParent(transform);
            }
        }

        public void OnPlayerLeave(GameObject player)
        {
            if (player != null)
            {
                player.transform.SetParent(null);
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                if (IsTopCollision(collision))
                {
                    OnPlayerLand(collision.gameObject);
                }
            }
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                OnPlayerLeave(collision.gameObject);
            }
        }

        private bool IsTopCollision(Collision2D collision)
        {
            foreach (ContactPoint2D contact in collision.contacts)
            {
                if (contact.normal.y < -0.5f)
                {
                    return true;
                }
            }
            return false;
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (_properties != null && _properties.IsMoving)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawLine(_startPosition, _endPosition);
                Gizmos.DrawWireSphere(_startPosition, 0.2f);
                Gizmos.DrawWireSphere(_endPosition, 0.2f);
            }
        }
#endif
    }
} 