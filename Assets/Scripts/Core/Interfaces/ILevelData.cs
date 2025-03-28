using UnityEngine;

namespace GameJamPlatformer.Core.Interfaces
{
    /// <summary>
    /// Data contract for level objects
    /// </summary>
    public interface ILevelObject
    {
        string PrefabId { get; }
        Vector2 Position { get; }
        float Rotation { get; }
        Vector2 Scale { get; }
    }

    /// <summary>
    /// Data contract for platform objects
    /// </summary>
    public interface IPlatformData : ILevelObject
    {
        bool IsMoving { get; }
        string MovementType { get; }
        float MovementSpeed { get; }
        float MovementDistance { get; }
        Vector2 MovementStartPoint { get; }
    }

    /// <summary>
    /// Data contract for level data
    /// </summary>
    public interface ILevelData
    {
        string LevelName { get; }
        Vector2 StartPoint { get; }
        Vector2 EndPoint { get; }
        ILevelObject[] Objects { get; }
    }
} 