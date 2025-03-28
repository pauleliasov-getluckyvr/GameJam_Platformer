using UnityEngine;

namespace GameJamPlatformer
{
    /// <summary>
    /// Base class for all platform types in the game
    /// </summary>
    public abstract class BasePlatform : MonoBehaviour, IPlatform
    {
        [SerializeField] protected PlatformProperties _properties;
        protected bool _isInitialized;
        protected Rigidbody2D _rigidbody;
        protected BoxCollider2D _collider;

        public PlatformProperties Properties => _properties;

        protected virtual void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _collider = GetComponent<BoxCollider2D>();

            if (_rigidbody == null)
            {
                _rigidbody = gameObject.AddComponent<Rigidbody2D>();
                _rigidbody.bodyType = RigidbodyType2D.Kinematic;
            }

            if (_collider == null)
            {
                _collider = gameObject.AddComponent<BoxCollider2D>();
            }
        }

        protected virtual void Start()
        {
            if (!_isInitialized && _properties != null)
            {
                Initialize(_properties);
            }
        }

        public virtual void Initialize(PlatformProperties properties)
        {
            _properties = properties;
            _isInitialized = true;
        }

        public virtual void UpdatePlatform()
        {
            // Base implementation does nothing
        }

        public virtual void OnPlayerLand(GameObject player)
        {
            // Base implementation does nothing
        }

        public virtual void OnPlayerLeave(GameObject player)
        {
            // Base implementation does nothing
        }

        protected virtual void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                OnPlayerLand(collision.gameObject);
            }
        }

        protected virtual void OnCollisionExit2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                OnPlayerLeave(collision.gameObject);
            }
        }

        protected virtual void OnDrawGizmos()
        {
            if (_properties != null && _properties.IsMoving && _properties.WayPoints != null)
            {
                Gizmos.color = Color.yellow;
                for (int i = 0; i < _properties.WayPoints.Length; i++)
                {
                    Vector2 current = _properties.WayPoints[i];
                    Vector2 next = _properties.WayPoints[(i + 1) % _properties.WayPoints.Length];
                    
                    Gizmos.DrawWireSphere(current, 0.2f);
                    Gizmos.DrawLine(current, next);
                }
            }
        }
    }
} 