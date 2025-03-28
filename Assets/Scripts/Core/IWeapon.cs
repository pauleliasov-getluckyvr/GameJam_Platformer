/// <summary>
/// Interface defining the contract for weapon behavior
/// </summary>
public interface IWeapon
{
    /// <summary>
    /// The name of the weapon
    /// </summary>
    string WeaponName { get; }

    /// <summary>
    /// The current ammo count
    /// </summary>
    int CurrentAmmo { get; }

    /// <summary>
    /// The maximum ammo capacity
    /// </summary>
    int MaxAmmo { get; }

    /// <summary>
    /// The damage dealt by this weapon
    /// </summary>
    float Damage { get; }

    /// <summary>
    /// The rate of fire in shots per second
    /// </summary>
    float FireRate { get; }

    /// <summary>
    /// Whether the weapon can be fired
    /// </summary>
    bool CanFire { get; }

    /// <summary>
    /// Fire the weapon in the specified direction
    /// </summary>
    /// <param name="direction">Direction to fire in</param>
    /// <returns>True if the weapon was fired successfully</returns>
    bool Fire(UnityEngine.Vector2 direction);

    /// <summary>
    /// Reload the weapon
    /// </summary>
    void Reload();

    /// <summary>
    /// Add ammo to the weapon
    /// </summary>
    /// <param name="amount">Amount of ammo to add</param>
    void AddAmmo(int amount);
} 