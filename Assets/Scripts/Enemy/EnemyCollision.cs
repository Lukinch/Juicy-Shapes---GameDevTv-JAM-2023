using UnityEngine;
using UnityEngine.Events;

public class EnemyCollision : MonoBehaviour, IDamageable
{
    [SerializeField] private EnemyColorManager _colorManager;
    [SerializeField] private float _damage = 10;

    public EnemyColorManager ColorManager => _colorManager;

    // Called in the editor
    public UnityEvent<float> OnDamageTaken;

    // If collides with the player, do damage (check physics matrix)
    void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent(out IDamageable damageable))
            return;

        damageable.DoDamage(_damage);
    }

    // If collides with a projectile, the projectile will call this
    public void DoDamage(float damageAmount)
    {
        OnDamageTaken?.Invoke(damageAmount);
    }
}
