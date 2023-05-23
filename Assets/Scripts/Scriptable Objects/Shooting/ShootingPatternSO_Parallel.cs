using System;
using GlobalEnums;
using UnityEngine;

/// <summary>
/// Scriptable Object in charge of firing projectiles in parallel
/// </summary>
[CreateAssetMenu(
    menuName = "Project Juicer/Shooting Pattern/Parallel",
    fileName = "Shooting Pattern Parallel")]
public class ShootingPatternSO_Parallel : ShootingPatternSO
{
    [SerializeField, Range(0.001f, 1f)]
    [Tooltip("Distance between parallel bullets per shot")]
    private float _distanceBetweenLines = 0.2f;
    [SerializeField, Min(0.1f)]
    [Tooltip("The offset from the player center. Treat the minimum of 1 as the original position.")]
    private float _shotPositionOffset = 1.3f;

    public override void Fire(Transform playerTransform, GameObject prefab, int shotsAmount, float damage, ThemeColor themeColor)
    {
        // Projectile lines should always be odd, so at least 1 bullet is shot in the direction of the cursor
        if (shotsAmount % 2 == 0) shotsAmount -= 1;
        // The 'shotsAmount - 1' is to evenly distribute the lines
        float linesMaxDistance = (shotsAmount - 1) * _distanceBetweenLines;

        // With the distance half I can start from -distanceHalf and add '_distanceBetweenLines'
        // in each 'for' iteration
        float distanceHalf = linesMaxDistance / 2f;

        float currentLinePosition = -distanceHalf;

        // Get the local right direction of the firingPoint
        Vector3 localRight = playerTransform.transform.right;
        Vector3 newPosition;
        for (int i = 0; i < shotsAmount; i++)
        {
            // Apply the local offset in the local X-axis direction of the firingPoint
            newPosition = playerTransform.position + (localRight * currentLinePosition) + (playerTransform.forward * _shotPositionOffset);

            _poolingManagerSO.PoolProjectile(prefab, newPosition, playerTransform.rotation, damage, themeColor);

            currentLinePosition += _distanceBetweenLines;
        }
    }

    /* Shooting approach explanation
        Implement a parallel shooting pattern
        Example with 5 projectile lines
        (projectileLines - 1) * _distanceBetweenLines = (5-1) * 0.05 = 0.2 meters
        between each projectile line, there has to be 1 line
        coming out from arrow tip (the player front).

        The 'projectileLines - 1' fix is to evenly distribute the distance between each line and
        having 1 line directly in front of the arrow tip (the player front)

        Without fix example:
        (5) * 0.05 = 0.25
        distanceHalf = linesMaxDistance / 2 = 0.25/2 = 0.125
        currentLinePosition = (-distanceHalf) = -0.125
        -0.125 (First projectile spawn)
        -0.125 + 0.05 = -0.075 (Second projectile spawn)
        -0.075 + 0.05 = -0.025 (Third projectile spawn) !!! The middle projectile should always be at 0
        -0.025 + 0.05 = 0.025 (Fourth projectile spawn)
        0.025 + 0.05 = 0.075 (Fifth projectile spawn) !!! The final projectile X position
        should be at 0.125
        -------------------------------------------------------------------
        Now with the fix:
        (5-1) * 0.05 = 0.2
        distanceHalf = linesMaxDistance / 2 = 0.2/2 = 0.1
        currentLinePosition = (-distanceHalf) = -0.1
        -0.1 (First projectile spawn)
        -0.1 + 0.05 = -0.05 (Second projectile spawn)
        -0.05 + 0.05 = 0 (Third projectile spawn)
        0 + 0.05 = 0.05 (Fourth projectile spawn)
        0.05 + 0.05 = 0.1 (Fifth projectile spawn)
    */
}
