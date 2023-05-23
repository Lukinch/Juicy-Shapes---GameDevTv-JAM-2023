using System.Collections;
using UnityEngine;
using NaughtyAttributes;
using GlobalEnums;
using System;

/// <summary>
/// Class in charge of reading 'Fire' input and calling the 'Pattern' SO to fire.
/// </summary>
public class PlayerFire : MonoBehaviour
{
    [Header("Input")]
    [SerializeField, Tooltip("Scriptable Object from which each input is read")]
    private InputReaderSO _inputReader;

    [Header("Dependencies")]
    [SerializeField, Tooltip("From where the bullets will spawn")]
    private Transform _visuals;

    [Header("Configuration")]
    [SerializeField, Tooltip("Scriptable Object containing player stats"), Expandable]
    private PlayerFiringStatsSO _playerFiringStats;
    [SerializeField, Tooltip("Scriptable Object containing the firing pattern"), Expandable]
    private ShootingPatternSO _shootingPattern;
    [SerializeField, Tooltip("The Scriptable Object containing a projectile information"), Expandable]
    private ProjectileSO _projectile;

    /// <summary>Current firing input state</summary>
    private bool _isFiring = false;
    /// <summary>Used to prevent input spamming</summary>
    private bool _isOnCooldown = false;
    /// <summary>Cached variable to save memory</summary>
    private WaitForSeconds _waitForSeconds;

    private ShootingPatternSO _initialPattern;
    private int _shotsAmount;
    private int _shotsPerSecond;
    private float _damage;
    private ThemeColor _themeColor;

    // Stats
    public int ShotsAmount
    {
        get => _shotsAmount;
        set => _shotsAmount = value;
    }
    public int ShotsPerSecond
    {
        get => _shotsPerSecond;
        set => _shotsPerSecond = value;
    }
    public float Damage
    {
        get => _damage;
        set => _damage = value;
    }
    public ShootingPatternSO ShootingPattern
    {
        get => _shootingPattern;
        set => _shootingPattern = value;
    }

    void Awake()
    {
        _initialPattern = _shootingPattern;
        _themeColor = ThemeColor.Red;
    }

    private void OnEnable()
    {
        ResetStats();

        _inputReader.OnFired += Input_OnFired;
        _inputReader.OnColorRedPressed += Input_OnRedPressed;
        _inputReader.OnColorPinkPressed += Input_OnPinkPressed;
        _inputReader.OnColorLightBluePressed += Input_OnLightBluePressed;
    }

    private void OnDisable()
    {
        _inputReader.OnFired -= Input_OnFired;
        _inputReader.OnColorRedPressed -= Input_OnRedPressed;
        _inputReader.OnColorPinkPressed -= Input_OnPinkPressed;
        _inputReader.OnColorLightBluePressed -= Input_OnLightBluePressed;
    }

    /// <summary>Only called once per input trigger</summary>
    private void Input_OnFired(bool isFiring)
    {
        _isFiring = isFiring;
        if (!_isOnCooldown)
            StartCoroutine(InitiateShot());
    }

    private void Input_OnRedPressed()
    {
        _themeColor = ThemeColor.Red;
    }

    private void Input_OnPinkPressed()
    {
        _themeColor = ThemeColor.Pink;
    }

    private void Input_OnLightBluePressed()
    {
        _themeColor = ThemeColor.LightBlue;
    }

    /// <summary>
    /// Sets cooldown to true and starts calling 'Fire' from a ShootingPatternSO's
    /// 'Fire' function.
    /// </summary>
    private IEnumerator InitiateShot()
    {
        _isOnCooldown = true;

        _waitForSeconds = new(1 / (float)_shotsPerSecond);

        while (_isFiring == true)
        {
            ShootingPattern.Fire(_visuals, _projectile.ProjectilePrefab, _shotsAmount, _damage, _themeColor);

            yield return _waitForSeconds;
        }

        _isOnCooldown = false;
    }

    public void ResetStats()
    {
        _shootingPattern = _initialPattern;
        _shotsAmount = _playerFiringStats.ProjectileLines;
        _shotsPerSecond = _playerFiringStats.ShotsPerSecond;
        _damage = _playerFiringStats.Damage;
        _themeColor = ThemeColor.Red;
    }
}
