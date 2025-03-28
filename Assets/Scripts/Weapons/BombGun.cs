using UnityEngine;
using UnityEngine.Pool;
using System.Collections.Generic;

namespace GameJamPlatformer.Weapons
{
    /// <summary>
    /// Bomb Gun weapon that fires explosive projectiles
    /// </summary>
    public class BombGun : BaseWeapon
    {
        [Header("Bomb Gun Settings")]
        [SerializeField] private GameObject bombPrefab;
        [SerializeField] private float launchForce = 10f;
        [SerializeField] private float explosionRadius = 3f;
        [SerializeField] private float explosionForce = 500f;
        [SerializeField] private LayerMask explosionLayers;

        private ObjectPool<GameObject> _bombPool;
        private List<Bomb> _activeBombs = new List<Bomb>();

        protected override void Awake()
        {
            base.Awake();
            weaponName = "Bomb Gun";
            damage = 50f;
            fireRate = 0.5f;
            InitializeBombPool();
        }

        private void InitializeBombPool()
        {
            _bombPool = new ObjectPool<GameObject>(
                createFunc: () => Instantiate(bombPrefab),
                actionOnGet: (obj) => obj.SetActive(true),
                actionOnRelease: (obj) => obj.SetActive(false),
                actionOnDestroy: (obj) => Destroy(obj),
                defaultCapacity: 10,
                maxSize: 20
            );
        }

        protected override void OnFire()
        {
            // Create bomb projectile
            GameObject bomb = Instantiate(bombPrefab, firePoint.position, firePoint.rotation);
            
            // Set bomb properties
            Rigidbody2D bombRb = bomb.GetComponent<Rigidbody2D>();
            if (bombRb != null)
            {
                bombRb.AddForce(aimDirection * launchForce, ForceMode2D.Impulse);
            }

            // Add explosion component
            Bomb bombComponent = bomb.GetComponent<Bomb>();
            if (bombComponent == null)
            {
                bombComponent = bomb.AddComponent<Bomb>();
            }

            bombComponent.Initialize(damage, explosionRadius, explosionForce, explosionLayers);
        }

        private void OnValidate()
        {
            if (bombPrefab == null)
            {
                Debug.LogWarning("BombGun: Bomb prefab is not assigned!");
            }
        }

        private void OnDrawGizmosSelected()
        {
            // Draw explosion radius preview
            Gizmos.color = new Color(1f, 0f, 0f, 0.2f);
            Gizmos.DrawWireSphere(firePoint.position, explosionRadius);
        }

        public override void AlternativeFire()
        {
            // Detonate all active bombs
            foreach (var bomb in _activeBombs.ToArray())
            {
                if (bomb != null)
                {
                    bomb.Explode();
                }
            }
            _activeBombs.Clear();
        }
    }

    /// <summary>
    /// Component that handles bomb behavior and explosion
    /// </summary>
    public class Bomb : MonoBehaviour
    {
        private float _damage;
        private float _explosionRadius;
        private float _explosionForce;
        private LayerMask _explosionLayers;
        private bool _hasExploded;

        public void Initialize(float damage, float radius, float force, LayerMask layers)
        {
            _damage = damage;
            _explosionRadius = radius;
            _explosionForce = force;
            _explosionLayers = layers;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (!_hasExploded)
            {
                Explode();
            }
        }

        public void Explode()
        {
            if (_hasExploded) return;
            
            _hasExploded = true;

            // Find objects in explosion radius
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, _explosionRadius, _explosionLayers);
            
            foreach (Collider2D hit in colliders)
            {
                // Apply damage if possible
                var damageable = hit.GetComponent<ICharacter>();
                if (damageable != null)
                {
                    // Calculate damage falloff based on distance
                    float distance = Vector2.Distance(transform.position, hit.transform.position);
                    float damageMultiplier = 1f - (distance / _explosionRadius);
                    damageable.TakeDamage(_damage * damageMultiplier);
                }

                // Apply explosion force
                Rigidbody2D rb = hit.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    Vector2 direction = (hit.transform.position - transform.position).normalized;
                    rb.AddForce(direction * _explosionForce, ForceMode2D.Impulse);
                }
            }

            // TODO: Spawn explosion effect
            
            // Destroy the bomb
            Destroy(gameObject);
        }

        private void OnDrawGizmos()
        {
            if (Application.isPlaying && !_hasExploded)
            {
                Gizmos.color = new Color(1f, 0f, 0f, 0.2f);
                Gizmos.DrawWireSphere(transform.position, _explosionRadius);
            }
        }
    }

    /// <summary>
    /// Interface for destructible objects
    /// </summary>
    public interface IDestructible
    {
        void OnDestroy();
    }
} 