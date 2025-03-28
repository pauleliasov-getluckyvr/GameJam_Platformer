using System;
using UnityEngine;

namespace GameJamPlatformer.Core
{
    /// <summary>
    /// Defines properties for platform movement and behavior
    /// </summary>
    [Serializable]
    public class PlatformProperties
    {
        public bool isMoving;
        public string movementType;
        public float speed;
        public float distance;
    }
} 