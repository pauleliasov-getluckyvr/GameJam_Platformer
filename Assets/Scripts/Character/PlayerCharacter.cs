using UnityEngine;
using GameJamPlatformer.Core.Interfaces;
using System.Collections.Generic;

namespace GameJamPlatformer.Character
{
    /// <summary>
    /// Main player character implementation
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(BoxCollider2D))]
    public class PlayerCharacter : MonoBehaviour, ICharacter
    {
        [Header("Movement Settings")]
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float jumpForce = 10f;
        [SerializeField] private float groundCheckDistance = 0.1f;
        [SerializeField] private LayerMask groundLayer;

        [Header("Character Settings")]
        [SerializeField] private float maxHealth = 100f;
        [SerializeField] private float currentHealth;

        [Header("Weapon Settings")]
        [SerializeField] private Transform weaponMount;
        [SerializeField] private float weaponRotationLimit = 180f;

        private Rigidbody2D _rb;
        private BoxCollider2D _collider;
        private bool _canDoubleJump;
        private bool _hasDoubleJumpPowerup;
        private List<IWeapon> _weapons;
        private int _currentWeaponIndex;
        private bool _isDead;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _collider = GetComponent<BoxCollider2D>();
            _weapons = new List<IWeapon>();
            currentHealth = maxHealth;
            InitializeWeapons();
        }

        private void InitializeWeapons()
        {
            // Initialize weapons (to be implemented)
        }

        public void Move(float direction)
        {
            if (_isDead) return;
            
            _rb.velocity = new Vector2(direction * moveSpeed, _rb.velocity.y);
            
            // Flip character based on movement direction
            if (direction != 0)
            {
                transform.localScale = new Vector3(Mathf.Sign(direction), 1, 1);
            }
        }

        public void Jump()
        {
            if (_isDead) return;

            if (IsGrounded())
            {
                _rb.velocity = new Vector2(_rb.velocity.x, jumpForce);
                _canDoubleJump = _hasDoubleJumpPowerup;
            }
        }

        public bool TryDoubleJump()
        {
            if (_isDead) return false;

            if (_canDoubleJump)
            {
                _rb.velocity = new Vector2(_rb.velocity.x, jumpForce);
                _canDoubleJump = false;
                return true;
            }
            return false;
        }

        public void UpdateAim(Vector2 aimDirection)
        {
            if (_isDead) return;

            if (_weapons.Count > 0 && _currentWeaponIndex < _weapons.Count)
            {
                IWeapon currentWeapon = _weapons[_currentWeaponIndex];
                currentWeapon.UpdateAim(aimDirection);
                
                // Update weapon mount rotation
                if (weaponMount != null)
                {
                    float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
                    angle = Mathf.Clamp(angle, -weaponRotationLimit / 2f, weaponRotationLimit / 2f);
                    weaponMount.rotation = Quaternion.Euler(0f, 0f, angle);
                }
            }
        }

        public void FireWeapon()
        {
            if (_isDead) return;

            if (_weapons.Count > 0 && _currentWeaponIndex < _weapons.Count)
            {
                IWeapon currentWeapon = _weapons[_currentWeaponIndex];
                if (currentWeapon.CanFire)
                {
                    currentWeapon.Fire();
                }
            }
        }

        public void EquipWeapon(IWeapon weapon)
        {
            if (_isDead) return;

            if (weapon != null)
            {
                _weapons.Add(weapon);
                _currentWeaponIndex = _weapons.Count - 1;
            }
        }

        public void SwitchWeapon()
        {
            if (_isDead) return;

            if (_weapons.Count > 1)
            {
                _currentWeaponIndex = (_currentWeaponIndex + 1) % _weapons.Count;
            }
        }

        public Vector2 GetPosition()
        {
            return transform.position;
        }

        public bool IsGrounded()
        {
            RaycastHit2D hit = Physics2D.Raycast(
                _collider.bounds.center,
                Vector2.down,
                _collider.bounds.extents.y + groundCheckDistance,
                groundLayer
            );
            return hit.collider != null;
        }

        public void TakeDamage(float damage)
        {
            if (_isDead) return;

            currentHealth = Mathf.Max(0f, currentHealth - damage);
            
            if (currentHealth <= 0f)
            {
                Die();
            }
        }

        public void Die()
        {
            if (_isDead) return;

            _isDead = true;
            _rb.velocity = Vector2.zero;
            _rb.isKinematic = true;
            _collider.enabled = false;

            // Notify any listeners about death
            GameEvents.OnPlayerDeath?.Invoke();
        }

        public void CollectItem(string itemType)
        {
            if (_isDead) return;

            switch (itemType)
            {
                case "DoubleJump":
                    _hasDoubleJumpPowerup = true;
                    break;
                case "Health":
                    currentHealth = Mathf.Min(maxHealth, currentHealth + 25f);
                    break;
                // Add other collectible types here
            }
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            // Draw ground check ray in editor
            if (_collider != null)
            {
                Gizmos.color = Color.green;
                Vector3 rayStart = _collider.bounds.center;
                Gizmos.DrawLine(rayStart, rayStart + Vector3.down * (_collider.bounds.extents.y + groundCheckDistance));
            }
        }
#endif
    }
} 