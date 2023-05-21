using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerHitCollision : MonoBehaviour
{
    public UnityEvent<float> OnDamageTaken;

    private void OnTriggerEnter(Collider other)
    {
        // damage for now will be 15
        OnDamageTaken?.Invoke(15f);
    }
}
