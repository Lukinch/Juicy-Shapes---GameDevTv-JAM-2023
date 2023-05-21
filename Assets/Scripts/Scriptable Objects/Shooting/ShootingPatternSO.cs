using UnityEngine;

/// <summary>
/// Base Scriptable Object for expansion to create multiple types of firing patterns
/// </summary>
public abstract class ShootingPatternSO : ScriptableObject
{
    public string PatternName;

    /// <summary>The pooling manager from which each pattern will ask for projectiles.</summary>
    [SerializeField, Tooltip("The pooling manager from which each pattern will ask for projectiles.")]
    protected PoolingManager _poolingManagerSO;

    /// <summary>Instantiates projectiles with a particular behavior.<br/>
    /// <paramref name="playerTransform"/>: Player position.<br/>
    /// <paramref name="prefab"/>: Projectile to instantiate.<br/>
    /// <paramref name="shotsAmount"/>: The amount of lines in which the projectiles will spawn (multiple bullets per shot).<br/>
    /// <paramref name="damage"/>: The damage that each projectile will make.<br/>
    /// </summary>
    public abstract void Fire(Transform playerTransform, GameObject prefab, int shotsAmount, float damage);
}
