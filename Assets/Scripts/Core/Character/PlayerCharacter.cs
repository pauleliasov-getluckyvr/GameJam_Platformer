using UnityEngine;
using GameJamPlatformer.Core.Interfaces;

namespace GameJamPlatformer
{
    /// <summary>
    /// Main player character controller implementing ICharacter interface
    /// </summary>
    public class PlayerCharacter : MonoBehaviour, ICharacter
    {
        [Header("Movement Settings")]
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float jumpForce = 12f;
        [SerializeField] private float doubleJumpForce = 8f;
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private float groundCheckRadius = 0.2f;

        [Header("Health Settings")]
        [SerializeField] private float maxHealth = 100f;
        [SerializeField] private bool isInvulnerable = false;

        [Header("Weapon Settings")]
        [SerializeField] private Transform weaponPivot;
        [SerializeField] private BaseWeapon[] availableWeapons;

        private Rigidbody2D _rb;
        private float _currentHealth;
        private bool _canDoubleJump = false;
        private IWeapon _currentWeapon;
        private int _currentWeaponIndex = 0;
        private Vector2 _aimDirection = Vector2.right;

        #region Unity Lifecycle

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _currentHealth = maxHealth;
        }

        private void Start()
        {
            // Equip first weapon if available
            if (availableWeapons != null && availableWeapons.Length > 0)
            {
                EquipWeapon(availableWeapons[0]);
            }
        }

        private void OnDrawGizmos()
        {
            // Draw ground check
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, groundCheckRadius);

            // Draw aim direction
            if (weaponPivot != null)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(weaponPivot.position, (Vector2)weaponPivot.position + _aimDirection);
            }
        }

        #endregion

        #region ICharacter Implementation

        public void Move(float direction)
        {
            if (_rb != null)
            {
                _rb.velocity = new Vector2(direction * moveSpeed, _rb.velocity.y);
                
                // Flip character based on movement direction
                if (direction != 0)
                {
                    transform.localScale = new Vector3(Mathf.Sign(direction), 1f, 1f);
                }
            }
        }

        public void Jump()
        {
            if (IsGrounded())
            {
                _rb.velocity = new Vector2(_rb.velocity.x, jumpForce);
                _canDoubleJump = true;
            }
        }

        public bool TryDoubleJump()
        {
            if (_canDoubleJump && !IsGrounded())
            {
                _rb.velocity = new Vector2(_rb.velocity.x, doubleJumpForce);
                _canDoubleJump = false;
                return true;
            }
            return false;
        }

        public void UpdateAim(Vector2 aimDirection)
        {
            _aimDirection = aimDirection.normalized;
            if (weaponPivot != null)
            {
                float angle = Mathf.Atan2(_aimDirection.y, _aimDirection.x) * Mathf.Rad2Deg;
                weaponPivot.rotation = Quaternion.Euler(0f, 0f, angle);

                // Update weapon aim
                if (_currentWeapon != null)
                {
                    _currentWeapon.UpdateAim(_aimDirection);
                }
            }
        }

        public void FireWeapon()
        {
            if (_currentWeapon != null && _currentWeapon.CanFire)
            {
                _currentWeapon.Fire();
            }
        }

        public void SwitchWeapon()
        {
            if (availableWeapons == null || availableWeapons.Length == 0) return;

            _currentWeaponIndex = (_currentWeaponIndex + 1) % availableWeapons.Length;
            EquipWeapon(availableWeapons[_currentWeaponIndex]);
        }

        public Vector2 GetPosition()
        {
            return transform.position;
        }

        public bool IsGrounded()
        {
            return Physics2D.OverlapCircle(transform.position, groundCheckRadius, groundLayer);
        }

        public void CollectItem(string itemType)
        {
            // Implement item collection logic
            Debug.Log($"Collected item: {itemType}");
        }

        public void EquipWeapon(IWeapon weapon)
        {
            // Unequip current weapon if it's a MonoBehaviour
            if (_currentWeapon is MonoBehaviour currentWeaponBehaviour)
            {
                Destroy(currentWeaponBehaviour.gameObject);
            }

            // Store the new weapon interface
            _currentWeapon = weapon;

            // If the weapon is a MonoBehaviour, set it up in the scene
            if (weapon is MonoBehaviour weaponBehaviour && weaponPivot != null)
            {
                weaponBehaviour.transform.SetParent(weaponPivot, false);
                weaponBehaviour.transform.localPosition = Vector3.zero;
                weaponBehaviour.transform.localRotation = Quaternion.identity;
            }
        }

        public void TakeDamage(float damage)
        {
            if (isInvulnerable) return;

            _currentHealth = Mathf.Max(0f, _currentHealth - damage);
            
            if (_currentHealth <= 0f)
            {
                Die();
            }
        }

        public void Die()
        {
            // Implement death logic
            Debug.Log("Player died!");
            
            // Disable the character
            enabled = false;
            if (_rb != null)
            {
                _rb.simulated = false;
            }

            // Disable weapon if it's a MonoBehaviour
            if (_currentWeapon is MonoBehaviour weaponBehaviour)
            {
                weaponBehaviour.enabled = false;
            }

            // Disable the game object
            gameObject.SetActive(false);

            // TODO: Trigger game over or respawn logic
        }

        #endregion
    }
} 