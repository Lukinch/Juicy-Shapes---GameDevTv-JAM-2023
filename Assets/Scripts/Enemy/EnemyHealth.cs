using UnityEngine;
using UnityEngine.Events;
using NaughtyAttributes;
using System.Collections;
using System;

public class EnemyHealth : MonoBehaviour, IDamageable, IPoolable
{
    [SerializeField, Range(1f, 1000f)] private int _maxHealthPoints = 100;
    [SerializeField] private bool _shouldBeDamaged = true;
    [Space]
    [SerializeField] private Renderer _renderer;
    [SerializeField] private Color _normalColor;
    [SerializeField] private Color _hitColor;
    [SerializeField, Range(0.01f, 1f)]
    private float _hitFXDuration = 0.2f;

    private float _currentHealth;
    private Coroutine _hitVSFCoroutine;
    private WaitForSeconds _waitForFlashVFX;

    public PoolingManager PoolingManagerSO { get; set; }

    public UnityEvent<float, float> OnHealthChanged;
    public static event Action OnEnemyDied;

    private void Awake()
    {
        _waitForFlashVFX = new(_hitFXDuration);
    }

    private void OnEnable()
    {
        _renderer.material.color = _normalColor;
        _currentHealth = _maxHealthPoints;
    }

    private IEnumerator FlashOnHit()
    {
        _renderer.material.color = _hitColor;
        yield return _waitForFlashVFX;
        _renderer.material.color = _normalColor;
    }

    public void DoDamage(float damageAmount)
    {
        if (!_shouldBeDamaged) return;
        if (_currentHealth == 0f) return;

        HitVFX();
        _currentHealth -= damageAmount;
        if (_currentHealth <= 0f)
        {
            _currentHealth = 0f;
            ReturnToPool();
            OnEnemyDied?.Invoke();
            return;
        }

        OnHealthChanged?.Invoke(_currentHealth, _maxHealthPoints);
    }

    private void ReturnToPool()
    {
        PoolingManagerSO.ReturnObject(gameObject);
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
        _currentHealth = _maxHealthPoints;

        OnHealthChanged?.Invoke(_currentHealth, _maxHealthPoints);
    }
}
