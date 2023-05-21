using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour, IPoolable
{
    public PoolingManager PoolingManagerSO { get; set; }

    public float Damage { set; get; }
}
