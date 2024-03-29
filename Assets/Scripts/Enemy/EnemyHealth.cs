using System;
using UnityEngine;
using UnityEngine.Events;
using NaughtyAttributes;

public class EnemyHealth : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private EnemyPooling _enemyPooling;

    [Header("Customization")]
    [SerializeField, Range(1f, 1000f)] private int _maxHealthPoints = 100;
    [SerializeField] private bool _shouldBeDamaged = true;

    private float _currentHealth;

    public UnityEvent<float, float> OnHealthChanged;
    public UnityEvent OnDamageTaken;
    public static event Action OnEnemyDied;
    public static event Action<Vector3> OnEnemyDiedPosition;
    public static event Action<Vector3> OnEnemyDamaged;

    private void OnEnable()
    {
        _currentHealth = _maxHealthPoints;
    }

    public void DoDamage(float damageAmount)
    {
        OnEnemyDamaged?.Invoke(transform.position);
        if (!_shouldBeDamaged) return;
        if (_currentHealth == 0f) return;

        _currentHealth -= damageAmount;
        if (_currentHealth <= 0f)
        {
            _currentHealth = 0f;
            _enemyPooling.ReturnToPool();
            OnEnemyDied?.Invoke();
            OnEnemyDiedPosition?.Invoke(transform.position);
            return;
        }

        OnDamageTaken?.Invoke();

        OnHealthChanged?.Invoke(_currentHealth, _maxHealthPoints);
    }

    [Button]
    public void ResetHealth()
    {
        _currentHealth = _maxHealthPoints;

        OnHealthChanged?.Invoke(_currentHealth, _maxHealthPoints);
    }
}
