using UnityEngine;
using UnityEngine.Pool;

namespace GameJamPlatformer.Weapons
{
    /// <summary>
    /// Nail Gun weapon that fires nails in rapid succession
    /// </summary>
    public class NailGun : BaseWeapon
    {
        [Header("Nail Gun Settings")]
        [SerializeField] private GameObject nailPrefab;
        [SerializeField] private float nailSpeed = 15f;
        [SerializeField] private float nailLifetime = 2f;

        protected override void Awake()
        {
            base.Awake();
            weaponName = "Nail Gun";
            damage = 15f;
            fireRate = 2f;
        }

        protected override void OnFire()
        {
            // Create nail projectile
            GameObject nail = Instantiate(nailPrefab, firePoint.position, firePoint.rotation);
            
            // Set nail properties
            Rigidbody2D nailRb = nail.GetComponent<Rigidbody2D>();
            if (nailRb != null)
            {
                nailRb.velocity = aimDirection * nailSpeed;
            }

            // Destroy nail after lifetime
            Destroy(nail, nailLifetime);
        }

        private void OnValidate()
        {
            if (nailPrefab == null)
            {
                Debug.LogWarning("NailGun: Nail prefab is not assigned!");
            }
        }
    }

    /// <summary>
    /// Component to handle nail behavior
    /// </summary>
    public class Nail : MonoBehaviour
    {
        private ObjectPool<GameObject> _pool;
        private LayerMask _woodenWallLayer;
        private float _lifetime;
        private bool _isStuck;
        private Rigidbody2D _rb;

        public void Initialize(ObjectPool<GameObject> pool, LayerMask woodenWallLayer, float lifetime)
        {
            _pool = pool;
            _woodenWallLayer = woodenWallLayer;
            _lifetime = lifetime;
            _isStuck = false;
            _rb = GetComponent<Rigidbody2D>();
            
            Invoke(nameof(ReturnToPool), _lifetime);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            // Check if collided with wooden wall
            if (((1 << collision.gameObject.layer) & _woodenWallLayer) != 0)
            {
                StickToWall(collision);
            }
            else
            {
                ReturnToPool();
            }
        }

        private void StickToWall(Collision2D collision)
        {
            if (_isStuck) return;

            _isStuck = true;
            _rb.velocity = Vector2.zero;
            _rb.isKinematic = true;
            transform.parent = collision.transform;

            // Create a platform collider for the nail
            BoxCollider2D platformCollider = gameObject.AddComponent<BoxCollider2D>();
            platformCollider.size = new Vector2(0.5f, 0.1f);
            platformCollider.offset = new Vector2(0, 0.05f);
            platformCollider.usedByEffector = true;

            // Add platform effector
            PlatformEffector2D effector = gameObject.AddComponent<PlatformEffector2D>();
            effector.useOneWay = true;
        }

        private void ReturnToPool()
        {
            if (_pool != null)
            {
                // Clean up components
                if (_isStuck)
                {
                    Destroy(GetComponent<BoxCollider2D>());
                    Destroy(GetComponent<PlatformEffector2D>());
                }
                
                transform.parent = null;
                _rb.isKinematic = false;
                _isStuck = false;
                _pool.Release(gameObject);
            }
        }
    }
} 