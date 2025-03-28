using UnityEngine;
using GameJamPlatformer.Weapons;

namespace GameJamPlatformer
{
    /// <summary>
    /// Interface defining the contract for character behavior
    /// </summary>
    public interface ICharacter
    {
        /// <summary>
        /// Moves the character horizontally
        /// </summary>
        void Move(float direction);

        /// <summary>
        /// Makes the character jump
        /// </summary>
        void Jump();

        /// <summary>
        /// Attempts to perform a double jump if available
        /// </summary>
        /// <returns>True if double jump was performed</returns>
        bool TryDoubleJump();

        /// <summary>
        /// Updates the character's weapon aim
        /// </summary>
        void UpdateAim(Vector2 aimDirection);

        /// <summary>
        /// Fires the current weapon
        /// </summary>
        void FireWeapon();

        /// <summary>
        /// Switches to the next weapon
        /// </summary>
        void SwitchWeapon();

        /// <summary>
        /// Gets the character's current position
        /// </summary>
        Vector2 GetPosition();

        /// <summary>
        /// Checks if the character is grounded
        /// </summary>
        bool IsGrounded();

        /// <summary>
        /// Handles collecting items
        /// </summary>
        void CollectItem(string itemType);

        /// <summary>
        /// Equips a weapon to the character
        /// </summary>
        /// <param name="weapon">The weapon to equip</param>
        void EquipWeapon(IWeapon weapon);

        /// <summary>
        /// Applies damage to the character
        /// </summary>
        /// <param name="damage">Amount of damage to apply</param>
        void TakeDamage(float damage);

        /// <summary>
        /// Handles character death
        /// </summary>
        void Die();
    }
} 