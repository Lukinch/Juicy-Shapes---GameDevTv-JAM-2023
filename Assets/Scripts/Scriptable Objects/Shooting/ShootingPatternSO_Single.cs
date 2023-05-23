using System.Collections;
using System.Collections.Generic;
using GlobalEnums;
using UnityEngine;

/// <summary>
/// Scriptable Object in charge of firing a single projectile
/// </summary>
[CreateAssetMenu(
    menuName = "Project Juicer/Shooting Pattern/Single",
    fileName = "Shooting Pattern Single")]
public class ShootingPatternSO_Single : ShootingPatternSO
{
    [SerializeField, Min(0.1f)]
    [Tooltip("The offset from the player center. Treat the minimum of 1 as the original position.")]
    private float _shotPositionOffset = 0.6f;

    public override void Fire(Transform playerTransform, GameObject prefab, int shotsAmount, float damage, ThemeColor themeColor)
    {
        Vector3 newPosition = playerTransform.position + (playerTransform.forward * _shotPositionOffset);
        _poolingManagerSO.PoolProjectile(prefab, newPosition, playerTransform.rotation, damage, themeColor);
    }
}
