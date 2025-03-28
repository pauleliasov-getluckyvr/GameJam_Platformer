using UnityEngine;
using GameJamPlatformer.Core.Interfaces;

namespace GameJamPlatformer
{
    /// <summary>
    /// Base class for all weapons in the game
    /// </summary>
    public abstract class BaseWeapon : MonoBehaviour, IWeapon
    {
        [Header("Base Weapon Settings")]
        [SerializeField] protected Transform firePoint;

        protected string weaponName = "Base Weapon";
        protected float damage = 10f;
        protected float fireRate = 1f;
        protected float lastFireTime;
        protected Vector2 aimDirection = Vector2.right;

        public string WeaponName => weaponName;
        public float Damage => damage;
        public float FireRate => fireRate;
        public bool CanFire => Time.time >= lastFireTime + (1f / fireRate);

        protected virtual void Awake()
        {
            // Ensure fire point exists
            if (firePoint == null)
            {
                firePoint = transform;
            }
        }

        public void Fire()
        {
            if (!CanFire) return;
            
            OnFire();
            lastFireTime = Time.time;
        }

        public virtual void AlternativeFire()
        {
            // Optional alternative fire mode
        }

        public void UpdateAim(Vector2 direction)
        {
            aimDirection = direction.normalized;
        }

        public Vector2 GetPosition()
        {
            return firePoint != null ? firePoint.position : transform.position;
        }

        public Quaternion GetRotation()
        {
            return firePoint != null ? firePoint.rotation : transform.rotation;
        }

        protected abstract void OnFire();

        protected virtual void OnValidate()
        {
            if (firePoint == null)
            {
                Debug.LogWarning($"{GetType().Name}: Fire point is not assigned!");
            }
        }

        protected virtual void OnDrawGizmos()
        {
            if (!Application.isPlaying) return;

            // Draw aim direction
            Gizmos.color = Color.red;
            Vector2 startPos = GetPosition();
            Gizmos.DrawLine(startPos, startPos + aimDirection);

            // Draw fire point
            if (firePoint != null)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireSphere(firePoint.position, 0.1f);
            }
        }
    }
} 