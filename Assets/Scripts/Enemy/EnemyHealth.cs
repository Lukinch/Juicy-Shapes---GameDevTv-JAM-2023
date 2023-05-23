using UnityEngine;
using UnityEngine.Events;
using NaughtyAttributes;
using System.Collections;
using System;

public class EnemyHealth : MonoBehaviour, IPoolable
{
    [SerializeField, Range(1f, 1000f)] private int _maxHealthPoints = 100;
    [SerializeField] private bool _shouldBeDamaged = true;
    [Space]

    private float _currentHealth;

    public PoolingManager PoolingManagerSO { get; set; }

    public UnityEvent<float, float> OnHealthChanged;
    public UnityEvent OnDamageTaken;
    public static event Action OnEnemyDied;

    private void OnEnable()
    {
        _currentHealth = _maxHealthPoints;
    }

    public void DoDamage(float damageAmount)
    {
        if (!_shouldBeDamaged) return;
        if (_currentHealth == 0f) return;

        OnDamageTaken?.Invoke();
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

    [Button]
    public void ResetHealth()
    {
        _currentHealth = _maxHealthPoints;

        OnHealthChanged?.Invoke(_currentHealth, _maxHealthPoints);
    }
}
