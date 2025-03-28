using UnityEngine;
using System.Collections;

namespace GameJamPlatformer
{
    /// <summary>
    /// Platform that moves between waypoints
    /// </summary>
    public class MovingPlatform : BasePlatform
    {
        private int _currentWaypointIndex;
        private float _waitTimer;
        private bool _isWaiting;
        private Vector2 _startPosition;
        private Coroutine _moveCoroutine;

        protected override void Start()
        {
            base.Start();
            _startPosition = transform.position;
            
            if (_properties != null && _properties.IsMoving && _properties.WayPoints != null && _properties.WayPoints.Length > 0)
            {
                StartMovement();
            }
        }

        public override void Initialize(PlatformProperties properties)
        {
            base.Initialize(properties);
            
            if (properties.IsMoving && properties.WayPoints != null && properties.WayPoints.Length > 0)
            {
                StartMovement();
            }
        }

        private void StartMovement()
        {
            if (_moveCoroutine != null)
            {
                StopCoroutine(_moveCoroutine);
            }
            _moveCoroutine = StartCoroutine(MoveRoutine());
        }

        private IEnumerator MoveRoutine()
        {
            while (true)
            {
                if (_properties.WayPoints.Length == 0)
                {
                    yield break;
                }

                Vector2 startPos = transform.position;
                Vector2 targetPos = _properties.WayPoints[_currentWaypointIndex];
                float journeyLength = Vector2.Distance(startPos, targetPos);
                float startTime = Time.time;

                while (Vector2.Distance(transform.position, targetPos) > 0.01f)
                {
                    float distanceCovered = (Time.time - startTime) * _properties.Speed;
                    float fractionOfJourney = distanceCovered / journeyLength;
                    transform.position = Vector2.Lerp(startPos, targetPos, fractionOfJourney);
                    yield return null;
                }

                transform.position = targetPos;

                if (_properties.WaitTime > 0)
                {
                    yield return new WaitForSeconds(_properties.WaitTime);
                }

                _currentWaypointIndex = (_currentWaypointIndex + 1) % _properties.WayPoints.Length;
            }
        }

        public override void OnPlayerLand(GameObject player)
        {
            base.OnPlayerLand(player);
            player.transform.SetParent(transform);
        }

        public override void OnPlayerLeave(GameObject player)
        {
            base.OnPlayerLeave(player);
            player.transform.SetParent(null);
        }

        private void OnDisable()
        {
            if (_moveCoroutine != null)
            {
                StopCoroutine(_moveCoroutine);
                _moveCoroutine = null;
            }
        }

#if UNITY_EDITOR
        protected override void OnDrawGizmos()
        {
            base.OnDrawGizmos();

            if (_properties != null && _properties.IsMoving && _properties.WayPoints != null)
            {
                // Draw the platform's current target
                if (_properties.WayPoints.Length > _currentWaypointIndex)
                {
                    Gizmos.color = Color.red;
                    Gizmos.DrawLine(transform.position, _properties.WayPoints[_currentWaypointIndex]);
                }
            }
        }
#endif
    }
} 