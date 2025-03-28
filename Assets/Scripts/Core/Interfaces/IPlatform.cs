using UnityEngine;

namespace GameJamPlatformer
{
    /// <summary>
    /// Properties for configuring platform behavior
    /// </summary>
    public class PlatformProperties
    {
        public bool IsMoving { get; set; }
        public bool IsDestructible { get; set; }
        public float Speed { get; set; }
        public float WaitTime { get; set; }
        public Vector2[] WayPoints { get; set; }
    }

    /// <summary>
    /// Interface for platform objects in the game
    /// </summary>
    public interface IPlatform
    {
        /// <summary>
        /// Gets the platform's properties
        /// </summary>
        PlatformProperties Properties { get; }

        /// <summary>
        /// Initializes the platform with the given properties
        /// </summary>
        /// <param name="properties">Platform configuration properties</param>
        void Initialize(PlatformProperties properties);

        /// <summary>
        /// Updates the platform's state
        /// </summary>
        void UpdatePlatform();

        /// <summary>
        /// Called when the player lands on the platform
        /// </summary>
        /// <param name="player">The player character that landed on the platform</param>
        void OnPlayerLand(GameObject player);

        /// <summary>
        /// Called when the player leaves the platform
        /// </summary>
        /// <param name="player">The player character that left the platform</param>
        void OnPlayerLeave(GameObject player);
    }
} 