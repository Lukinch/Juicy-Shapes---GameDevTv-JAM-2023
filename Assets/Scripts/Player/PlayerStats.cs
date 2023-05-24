using System;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private GameObject _playerVisuals;
    [SerializeField] private MeshRenderer _playerVisualsRenderer;
    [SerializeField] private InputReaderSO _inputReader;
    [SerializeField] private PlayerFiringStatsSO _playerFiringStats;
    [SerializeField] private PlayerMovementStatsSO _playerMovementStats;
    [SerializeField] private PlayerHealthStatsSO _playerHealthStats;
    [SerializeField] private PlayerMovement _playerMovement;
    [SerializeField] private PlayerFire _playerFire;
    [SerializeField] private PlayerHealth _playerHealth;

    void OnEnable()
    {
        PauseManager.OnGamePaused += PauseManager_OnGamePaused;
        PlayerHealth.OnPlayerDied += PlayerHealth_OnPlayerDied;
        EnemiesManager.OnWaveEnded += EnemiesManager_OnWaveEnded;
        EnemiesManager.OnAllWavesCleared += EnemiesManager_OnAllWavesCleared;
        GameStateManager.OnGameStarted += GameStateManager_OnGameStarted;
        GameStateManager.OnPlayAgain += GameStateManager_OnPlayAgain;
        GameStateManager.OnNextWaveCountdownFinished += GameStateManager_OnNextWaveCountdownFinished;
    }

    void OnDisable()
    {
        PauseManager.OnGamePaused -= PauseManager_OnGamePaused;
        PlayerHealth.OnPlayerDied -= PlayerHealth_OnPlayerDied;
        EnemiesManager.OnWaveEnded -= EnemiesManager_OnWaveEnded;
        EnemiesManager.OnAllWavesCleared -= EnemiesManager_OnAllWavesCleared;
        GameStateManager.OnGameStarted -= GameStateManager_OnGameStarted;
        GameStateManager.OnPlayAgain -= GameStateManager_OnPlayAgain;
        GameStateManager.OnNextWaveCountdownFinished -= GameStateManager_OnNextWaveCountdownFinished;
    }

    private void PauseManager_OnGamePaused(bool paused)
    {
        // if (paused) DisableControls();
        // else EnableControls();
    }

    private void PlayerHealth_OnPlayerDied()
    {
        DisableControls();
        _playerVisuals.SetActive(false);
    }

    private void EnemiesManager_OnAllWavesCleared()
    {
        DisableControls();
    }

    private void GameStateManager_OnGameStarted()
    {
        DisableControls();
    }

    private void EnemiesManager_OnWaveEnded(int arg1, int arg2)
    {
        DisableControls();
    }

    private void GameStateManager_OnPlayAgain()
    {
        DisableControls();
        _playerMovement.ResetStats();
        _playerFire.ResetStats();
        _playerHealth.ResetStats();
        _playerVisuals.SetActive(true);
        _playerVisualsRenderer.enabled = true;
    }

    private void GameStateManager_OnNextWaveCountdownFinished()
    {
        EnableControls();
    }

    private void EnableControls()
    {
        _inputReader.EnableInputActions();
    }

    private void DisableControls()
    {
        _inputReader.DisableInputActions();
    }

    public GameObject PlayerVisuals => _playerVisuals;
    public MeshRenderer PlayerVisualsRenderer => _playerVisualsRenderer;

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
    }
    public float CurrentHP
    {
        get => _playerHealth.MaxHealthPoints;
    }
    public void IncreaseMaxHPAndHeal(float percentageHeal, float addMaxHPAmount)
    {
        _playerHealth.IncreaseMaxHPAndHeal(percentageHeal, addMaxHPAmount);
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
