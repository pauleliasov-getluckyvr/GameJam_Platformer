using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

namespace GameJamPlatformer.Core
{
    /// <summary>
    /// Manages level state, completion conditions, and collectibles
    /// </summary>
    public class LevelManager : MonoBehaviour
    {
        [Header("Level Settings")]
        [SerializeField] private Transform startPoint;
        [SerializeField] private Transform endPoint;
        [SerializeField] private float completionCheckRadius = 1f;

        [Header("Events")]
        public UnityEvent onLevelStart;
        public UnityEvent onLevelComplete;
        public UnityEvent<string> onCollectible;

        private PlayerCharacter _player;
        private List<Collider2D> _collectibles;
        private bool _isLevelComplete;

        private void Awake()
        {
            _collectibles = new List<Collider2D>();
            _player = FindObjectOfType<PlayerCharacter>();
        }

        private void Start()
        {
            if (_player != null && startPoint != null)
            {
                _player.transform.position = startPoint.position;
                onLevelStart?.Invoke();
            }
            else
            {
                Debug.LogError("Missing player or start point reference!");
            }
        }

        private void Update()
        {
            if (!_isLevelComplete && IsPlayerAtEndPoint())
            {
                CompleteLevel();
            }
        }

        private bool IsPlayerAtEndPoint()
        {
            if (_player == null || endPoint == null) return false;

            float distance = Vector2.Distance(_player.transform.position, endPoint.position);
            return distance <= completionCheckRadius;
        }

        private void CompleteLevel()
        {
            _isLevelComplete = true;
            onLevelComplete?.Invoke();
            // Additional completion logic (save progress, unlock next level, etc.)
        }

        public void RegisterCollectible(Collider2D collectible)
        {
            if (!_collectibles.Contains(collectible))
            {
                _collectibles.Add(collectible);
            }
        }

        public void OnCollectibleCollected(Collider2D collectible, string type)
        {
            if (_collectibles.Contains(collectible))
            {
                _collectibles.Remove(collectible);
                onCollectible?.Invoke(type);
            }
        }

        private void OnDrawGizmos()
        {
            // Draw start point
            if (startPoint != null)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawWireSphere(startPoint.position, 0.5f);
            }

            // Draw end point and completion radius
            if (endPoint != null)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(endPoint.position, completionCheckRadius);
            }
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            // Ensure start and end points are properly tagged
            if (startPoint != null)
                startPoint.gameObject.tag = "StartPoint";
            if (endPoint != null)
                endPoint.gameObject.tag = "EndPoint";
        }
#endif
    }
} 