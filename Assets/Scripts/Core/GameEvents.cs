using System;
using UnityEngine;

/// <summary>
/// Static class for handling game-wide events
/// </summary>
public static class GameEvents
{
    // Character events
    public static Action OnPlayerDeath;
    public static Action<float> OnPlayerHealthChanged;
    public static Action<string> OnPlayerCollectItem;
    public static Action<IWeapon> OnPlayerEquipWeapon;

    // Level events
    public static Action<string> OnLevelStart;
    public static Action<string> OnLevelComplete;
    public static Action<string> OnCheckpointReached;

    // Game state events
    public static Action OnGamePause;
    public static Action OnGameResume;
    public static Action OnGameOver;
    public static Action OnGameRestart;

    /// <summary>
    /// Reset all events when switching scenes or restarting the game
    /// </summary>
    public static void ResetEvents()
    {
        OnPlayerDeath = null;
        OnPlayerHealthChanged = null;
        OnPlayerCollectItem = null;
        OnPlayerEquipWeapon = null;
        OnLevelStart = null;
        OnLevelComplete = null;
        OnCheckpointReached = null;
        OnGamePause = null;
        OnGameResume = null;
        OnGameOver = null;
        OnGameRestart = null;
    }
} 