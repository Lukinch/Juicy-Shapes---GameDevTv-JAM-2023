using UnityEngine;
using NaughtyAttributes;
using System.Collections;
using System;

public class PlayerHealth : MonoBehaviour
{
    [Header("Input")]
    [SerializeField, Tooltip("Scriptable Object from which each input is read")]
    private InputReaderSO _inputReader;

    [Header("Collision")]
    [SerializeField]
    private Collider _hitCollider;

    [Header("Configuration")]
    [SerializeField, Expandable]
    private PlayerHealthStatsSO _playerHealthStats;

    [Space]
    [SerializeField] private Renderer _renderer;
    [SerializeField] private Color _normalColor;
    [SerializeField] private Color _hitColor;
    [SerializeField, Range(0.01f, 5)]
    private float _cooldownBetweenHits = 2f;


    private float _currentHealth;
    private float _maxHealthPoints;

    private Coroutine _hitVFXCoroutine;
    private Coroutine _hitCooldownCoroutine;
    private WaitForSeconds _waitForCooldownHit;

    // Stats
    public float MaxHealthPoints
    {
        get => _maxHealthPoints;
        set
        {
            IncreaseMaxHPAndHeal(0, value);
        }
    }

    public static event Action<float, float> OnPlayerHealthChanged;
    public static event Action<float, float> OnResetHealth;
    public static event Action OnPlayerDied;
    public static event Action OnPlayerDamaged;

    private void Awake()
    {
        _waitForCooldownHit = new(_cooldownBetweenHits);
    }

    private void OnEnable()
    {
        ResetStats();
    }

    void OnDisable()
    {
        ResetStats();
    }

    private void HitVFX()
    {
        _hitVFXCoroutine = StartCoroutine(StartVFX());
        _hitCooldownCoroutine = StartCoroutine(WaitForHitCooldown());
    }

    private IEnumerator StartVFX()
    {
        _renderer.material.color = _hitColor;
        yield return _waitForCooldownHit;
        _renderer.material.color = _normalColor;
    }

    private IEnumerator WaitForHitCooldown()
    {
        yield return _waitForCooldownHit;
        _hitCollider.enabled = true;
    }

    private void StopCoroutines()
    {
        if (_hitVFXCoroutine != null)
            StopCoroutine(_hitVFXCoroutine);
        if (_hitCooldownCoroutine != null)
            StopCoroutine(_hitCooldownCoroutine);
        _renderer.material.color = _normalColor;
    }

    public void IncreaseMaxHPAndHeal(float percentageHeal, float addMaxHPAmount)
    {
        if (_currentHealth == _maxHealthPoints)
        {
            _maxHealthPoints += addMaxHPAmount;
            _currentHealth = MaxHealthPoints;
        }
        else
        {
            _maxHealthPoints += addMaxHPAmount;

            float healAmount = _maxHealthPoints * percentageHeal;

            // HealVFX(); // TODO: implement later
            _currentHealth += healAmount;

            if (_currentHealth >= MaxHealthPoints)
            {
                _currentHealth = MaxHealthPoints;
            }
        }

        OnPlayerHealthChanged?.Invoke(_currentHealth, _maxHealthPoints);
    }

    // Being called by an Unity Event
    public void DoDamage(float damageAmount)
    {
        if (_currentHealth == 0f) return;

        _hitCollider.enabled = false;

        _currentHealth -= damageAmount;

        if (_currentHealth <= 0f)
        {
            _inputReader.DisableInputActions();
            GetComponent<PlayerStats>().PlayerVisuals.SetActive(false);
            _currentHealth = 0f;
            ResetStats();
            OnPlayerDied?.Invoke();
        }

        if (_currentHealth > 0f)
        {
            HitVFX();
        }

        OnPlayerDamaged?.Invoke();
        OnPlayerHealthChanged?.Invoke(_currentHealth, _maxHealthPoints);
    }

    public void ResetStats()
    {
        _maxHealthPoints = _playerHealthStats.MaxHealthPoints;
        _currentHealth = _maxHealthPoints;
        _hitCollider.enabled = true;
        OnResetHealth?.Invoke(_currentHealth, _maxHealthPoints);
        StopCoroutines();
    }
}

