using UnityEngine;

[CreateAssetMenu(
    menuName = "Project Juicer/Player/Firing Stats",
    fileName = "Player Firing Stats")]
public class PlayerFiringStatsSO : ScriptableObject
{
    [Header("Firing Options")]
    [SerializeField, Range(1, 24)]
    [Tooltip("The number of multiple projectiles per shot in different directions")]
    private int _projectileLines = 1;
    [SerializeField, Range(1, 24)]
    [Tooltip("The number of multiple projectiles per shot in different directions")]
    private int _maxProjectileLines = 12;
    [SerializeField, Range(1, 120)]
    [Tooltip("The number of shots per second")]
    private int _shotsPerSecond = 10;
    [SerializeField, Range(1, 120)]
    [Tooltip("The number of shots per second")]
    private int _maxShotsPerSecond = 60;
    [SerializeField, Min(0)]
    [Tooltip("The amount of damage player projectiles will do")]
    private int _damage = 20;

    public int MaxProjectileLines => _maxProjectileLines;
    public int MaxShotsPerSecond => _maxShotsPerSecond;
    public int ProjectileLines => _projectileLines;
    public int ShotsPerSecond => _shotsPerSecond;
    public int Damage => _damage;
}
