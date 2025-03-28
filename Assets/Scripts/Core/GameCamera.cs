using UnityEngine;

namespace GameJamPlatformer.Core
{
    /// <summary>
    /// Handles camera setup and maintains proper resolution and boundaries
    /// </summary>
    [RequireComponent(typeof(Camera))]
    public class GameCamera : MonoBehaviour
    {
        [Header("Resolution Settings")]
        [SerializeField] private float targetAspect = 16f / 9f;
        [SerializeField] private float pixelsPerUnit = 100f;
        
        [Header("Camera Bounds")]
        [SerializeField] private Vector2 levelSize = new Vector2(19.2f, 10.8f); // 1920/100, 1080/100

        private Camera _camera;
        private float _originalOrthographicSize;

        private void Awake()
        {
            _camera = GetComponent<Camera>();
            _originalOrthographicSize = _camera.orthographicSize;
            SetupCamera();
        }

        private void SetupCamera()
        {
            // Calculate and set the proper orthographic size based on the target resolution
            float screenAspect = (float)Screen.width / Screen.height;
            
            if (screenAspect >= targetAspect)
            {
                // Screen is wider than target aspect - adjust height
                _camera.orthographicSize = levelSize.y / 2f;
            }
            else
            {
                // Screen is taller than target aspect - adjust width
                float differenceInSize = targetAspect / screenAspect;
                _camera.orthographicSize = levelSize.y / 2f * differenceInSize;
            }

            // Center the camera
            transform.position = new Vector3(levelSize.x / 2f, levelSize.y / 2f, transform.position.z);

            // Set camera properties for pixel perfect rendering
            _camera.orthographic = true;
            _camera.clearFlags = CameraClearFlags.SolidColor;
            _camera.backgroundColor = Color.black;
        }

        private void OnDrawGizmos()
        {
            // Draw level boundaries
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(new Vector3(levelSize.x / 2f, levelSize.y / 2f, 0f), 
                new Vector3(levelSize.x, levelSize.y, 0f));
        }

        public Bounds GetCameraBounds()
        {
            return new Bounds(
                new Vector3(levelSize.x / 2f, levelSize.y / 2f, 0f),
                new Vector3(levelSize.x, levelSize.y, 0f)
            );
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (Application.isPlaying) return;
            
            // Update camera settings in editor
            if (_camera == null)
                _camera = GetComponent<Camera>();
                
            SetupCamera();
        }
#endif
    }
} 