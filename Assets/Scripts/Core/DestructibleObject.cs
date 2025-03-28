using UnityEngine;
using GameJamPlatformer.Weapons;

namespace GameJamPlatformer.Core
{
    /// <summary>
    /// Base class for all destructible objects in the game
    /// </summary>
    public class DestructibleObject : MonoBehaviour, IDestructible
    {
        [Header("Destructible Settings")]
        [SerializeField] protected int health = 1;
        [SerializeField] protected string rewardType;
        [SerializeField] protected GameObject destroyEffectPrefab;
        [SerializeField] protected GameObject rewardPrefab;

        protected virtual void Awake()
        {
            // Add any necessary components
            if (GetComponent<Collider2D>() == null)
            {
                gameObject.AddComponent<BoxCollider2D>();
            }
        }

        public virtual void OnDestroy()
        {
            health--;
            
            if (health <= 0)
            {
                HandleDestruction();
            }
        }

        protected virtual void HandleDestruction()
        {
            // Spawn destroy effect
            if (destroyEffectPrefab != null)
            {
                Instantiate(destroyEffectPrefab, transform.position, Quaternion.identity);
            }

            // Spawn reward
            if (rewardPrefab != null)
            {
                Instantiate(rewardPrefab, transform.position, Quaternion.identity);
            }

            // Destroy the object
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Wooden wall that can be used with the nail gun
    /// </summary>
    public class WoodenWall : DestructibleObject
    {
        protected override void Awake()
        {
            base.Awake();
            gameObject.layer = LayerMask.NameToLayer("WoodenWall");
        }
    }

    /// <summary>
    /// Balloon that contains coins and can be destroyed by any weapon
    /// </summary>
    public class CoinBalloon : DestructibleObject
    {
        [SerializeField] private int coinAmount = 1;

        protected override void HandleDestruction()
        {
            // Spawn coins
            for (int i = 0; i < coinAmount; i++)
            {
                if (rewardPrefab != null)
                {
                    Vector2 randomOffset = Random.insideUnitCircle * 0.5f;
                    Vector3 spawnPos = transform.position + (Vector3)randomOffset;
                    Instantiate(rewardPrefab, spawnPos, Quaternion.identity);
                }
            }

            base.HandleDestruction();
        }
    }
} 