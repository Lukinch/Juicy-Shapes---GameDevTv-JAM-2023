using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerHitCollision : MonoBehaviour, IDamageable
{
    // Called in the editor
    public UnityEvent<float> OnDamageTaken;

    public void DoDamage(float damageAmount)
    {
        OnDamageTaken?.Invoke(damageAmount);
    }
}
