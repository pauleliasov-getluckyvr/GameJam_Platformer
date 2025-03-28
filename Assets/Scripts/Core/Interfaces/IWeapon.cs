using UnityEngine;

namespace GameJamPlatformer
{
    /// <summary>
    /// Interface defining the contract for weapon behavior
    /// </summary>
    public interface IWeapon
    {
        /// <summary>
        /// Gets the weapon's name
        /// </summary>
        string WeaponName { get; }

        /// <summary>
        /// Gets the weapon's damage
        /// </summary>
        float Damage { get; }

        /// <summary>
        /// Gets the weapon's fire rate in shots per second
        /// </summary>
        float FireRate { get; }

        /// <summary>
        /// Gets whether the weapon is ready to fire
        /// </summary>
        bool CanFire { get; }

        /// <summary>
        /// Fires the weapon
        /// </summary>
        void Fire();

        /// <summary>
        /// Updates the weapon's aim direction
        /// </summary>
        /// <param name="direction">The direction to aim</param>
        void UpdateAim(Vector2 direction);

        /// <summary>
        /// Gets the weapon's current position
        /// </summary>
        Vector2 GetPosition();

        /// <summary>
        /// Gets the weapon's current rotation
        /// </summary>
        Quaternion GetRotation();

        /// <summary>
        /// Fires the weapon in an alternative mode
        /// </summary>
        void AlternativeFire();
    }
} 