using UnityEngine;

namespace GameJamPlatformer
{
    /// <summary>
    /// Basic non-moving platform
    /// </summary>
    public class StaticPlatform : BasePlatform
    {
        protected override void Start()
        {
            base.Start();
            
            // Ensure the platform is static
            if (_properties == null)
            {
                _properties = new PlatformProperties
                {
                    IsMoving = false,
                    IsDestructible = false
                };
            }
            else
            {
                _properties.IsMoving = false;
            }
        }

        public override void Initialize(PlatformProperties properties)
        {
            // Override any movement settings
            properties.IsMoving = false;
            base.Initialize(properties);
        }

        public override void UpdatePlatform()
        {
            // Static platforms don't need updating
        }

        public override void OnPlayerLand(GameObject player)
        {
            base.OnPlayerLand(player);
            // Add any specific behavior for when a player lands on a static platform
        }

        public override void OnPlayerLeave(GameObject player)
        {
            base.OnPlayerLeave(player);
            // Add any specific behavior for when a player leaves a static platform
        }
    }
} 