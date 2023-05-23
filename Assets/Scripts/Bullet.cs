using System.Collections;
using UnityEngine;

/// <summary>Class that controls a bullet movement upon spawning and its death itself</summary>
public class Bullet : Projectile
{
    [SerializeField, Range(0.1f, 100f)]
    [Tooltip("m/s that the bullet will be translating")]
    private float _moveSpeed = 20f;
    [SerializeField]
    [Tooltip("If the bullet should move upon spawning.\nFor debugging purposes")]
    private bool _shouldMove = true;
    [SerializeField, Range(0.1f, 10f)]
    [Tooltip("Time after the bullet will destroy itself.")]
    private float _destroyAfter = 2f;

    Coroutine _waitForReturnToPoolCoroutine;

    void OnEnable()
    {
        _waitForReturnToPoolCoroutine = StartCoroutine(WaitForReturnToPool());
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent(out IDamageable damageable))
            return;
        if (!other.TryGetComponent(out EnemyCollision enemyCollision))
            return;

        if (enemyCollision.ColorManager.EnemyColor != ProjectileColor) return;

        if (_waitForReturnToPoolCoroutine != null)
        {
            StopCoroutine(_waitForReturnToPoolCoroutine);
            _waitForReturnToPoolCoroutine = null;
        }

        damageable.DoDamage(Damage);
        ReturnToPool();
    }

    void Update()
    {
        if (!_shouldMove) return;
        transform.position += Time.deltaTime * _moveSpeed * transform.forward;
    }

    void ReturnToPool()
    {
        // Do some fancy effect here later or something
        PoolingManagerSO.ReturnObject(gameObject);
    }

    IEnumerator WaitForReturnToPool()
    {
        yield return new WaitForSeconds(_destroyAfter);
        ReturnToPool();
    }
}
