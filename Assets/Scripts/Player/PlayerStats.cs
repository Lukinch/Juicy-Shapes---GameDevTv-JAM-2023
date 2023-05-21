using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private InputReaderSO _inputReader;
    [SerializeField] private PlayerFiringStatsSO _playerFiringStats;
    [SerializeField] private PlayerMovementStatsSO _playerMovementStats;
    [SerializeField] private PlayerHealthStatsSO _playerHealthStats;
    [SerializeField] private PlayerMovement _playerMovement;
    [SerializeField] private PlayerFire _playerFire;
    [SerializeField] private PlayerHealth _playerHealth;

    void OnEnable()
    {
        EnemiesManager.OnWaveEnded += DisableControls;
        ProgressionManager.OnUpgradeFinished += EnableControls;
    }

    private void EnableControls()
    {
        _inputReader.EnableInputActions();
    }

    private void DisableControls(int currentWave, int maxWaves)
    {
        _inputReader.DisableInputActions();
    }

    public ShootingPatternSO ShootingPattern
    {
        get => _playerFire.ShootingPattern;
        set => _playerFire.ShootingPattern = value;
    }
    public int ShotsAmount
    {
        get => _playerFire.ShotsAmount;
        set => _playerFire.ShotsAmount = value;
    }
    public int MaxShotsAmount
    {
        get => _playerFiringStats.MaxProjectileLines;
    }
    public int ShotsPerSecond
    {
        get => _playerFire.ShotsPerSecond;
        set => _playerFire.ShotsPerSecond = value;
    }
    public int MaxShotsPerSecond
    {
        get => _playerFiringStats.MaxShotsPerSecond;
    }
    public float Damage
    {
        get => _playerFire.Damage;
        set => _playerFire.Damage = value;
    }
    public float CurrentMaxHealthPoints
    {
        get => _playerHealth.MaxHealthPoints;
        set => _playerHealth.MaxHealthPoints = value;
    }
    public float MoveSpeed
    {
        get => _playerMovement.MoveSpeed;
        set => _playerMovement.MoveSpeed = value;
    }
    public float Acceleration
    {
        get => _playerMovement.Acceleration;
        set => _playerMovement.Acceleration = value;
    }
}
