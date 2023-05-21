using UnityEngine;
using UnityEngine.Events;
using NaughtyAttributes;
using System.Collections;
using System;

public class PlayerHealth : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField, Expandable]
    private PlayerHealthStatsSO _playerHealthStats;

    [Space]
    [SerializeField] private bool _shouldBeDamaged = true;
    [SerializeField] private Renderer _renderer;
    [SerializeField] private Color _normalColor;
    [SerializeField] private Color _hitColor;
    [SerializeField, Range(0.01f, 1f)]
    private float _hitFXDuration = 0.2f;

    private float _currentHealth;
    private float _maxHealthPoints;

    private Coroutine _hitVSFCoroutine;
    private WaitForSeconds _waitForFlashVFX;

    // Stats
    public float MaxHealthPoints
    {
        get => _maxHealthPoints;
        set
        {
            _maxHealthPoints = value;
            OnPlayerHealthChanged?.Invoke(_currentHealth, _maxHealthPoints);
        }
    }

    public UnityEvent<float, float> OnPlayerHealthChanged;

    private void Awake()
    {
        _waitForFlashVFX = new(_hitFXDuration);
    }

    private void OnEnable()
    {
        _maxHealthPoints = _playerHealthStats.MaxHealthPoints;
        _currentHealth = _maxHealthPoints;
    }

    private IEnumerator FlashOnHit()
    {
        _renderer.material.color = _hitColor;
        yield return _waitForFlashVFX;
        _renderer.material.color = _normalColor;
    }

    // Being called by an Unity Event
    public void DoDamage(float damageAmount)
    {
        if (!_shouldBeDamaged) return;
        if (_currentHealth == 0f) return;

        HitVFX();
        _currentHealth -= damageAmount;

        if (_currentHealth <= 0f)
        {
            _currentHealth = 0f;
            Destroy(gameObject);
            return;
        }

        OnPlayerHealthChanged?.Invoke(_currentHealth, _playerHealthStats.MaxHealthPoints);
    }

    // TODO: Should be called by an Unity Event in the future
    public void DoHeal(float healAmount)
    {
        if (_currentHealth == MaxHealthPoints) return;

        // HealVFX(); // TODO: implement later
        _currentHealth += healAmount;

        if (_currentHealth >= MaxHealthPoints)
        {
            _currentHealth = MaxHealthPoints;
        }

        OnPlayerHealthChanged?.Invoke(_currentHealth, MaxHealthPoints);
    }

    private void HitVFX()
    {
        if (_hitVSFCoroutine != null)
            StopCoroutine(_hitVSFCoroutine);

        _hitVSFCoroutine = StartCoroutine(FlashOnHit());
    }

    [Button]
    public void ResetHealth()
    {
        _currentHealth = _playerHealthStats.MaxHealthPoints;

        OnPlayerHealthChanged?.Invoke(_currentHealth, _playerHealthStats.MaxHealthPoints);
    }
}
