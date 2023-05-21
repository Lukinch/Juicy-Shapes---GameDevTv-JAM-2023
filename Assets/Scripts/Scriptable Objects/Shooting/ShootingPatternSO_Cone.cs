using System;
using UnityEngine;

/// <summary>
/// Scriptable Object in charge of firing projectiles in a cone
/// </summary>
[CreateAssetMenu(
    menuName = "Project Juicer/Shooting Pattern/Cone",
    fileName = "Shooting Pattern Cone")]
public class ShootingPatternSO_Cone : ShootingPatternSO
{
    [SerializeField, Range(5, 360)]
    [Tooltip("The cone shooting range in degrees.\n" +
    "This will be divided by 2, as Unity works with -180 to 180 degrees.")]
    private int _coneDegree = 60;
    [SerializeField, Min(0.1f)]
    [Tooltip("The offset from the player center. Treat the minimum of 1 as the original position.")]
    private float _shotPositionOffset = 1.3f;

    public override void Fire(Transform playerTransform, GameObject prefab, int shotsAmount, float damage)
    {
        // Projectile lines should always be odd, so at least 1 bullet is shot in the direction of the cursor
        if (shotsAmount % 2 == 0) shotsAmount -= 1;
        // The 'shotsAmount - 1' is to evenly distribute the lines along the cone
        float angleBetweenLines = _coneDegree / (float)(shotsAmount - 1);

        // With the cone half I can start from -coneHalf and add 'angleBetweenLines' in each
        // 'for' iteration
        float coneHalf = _coneDegree / 2f;

        float currentLineRotation = -coneHalf;

        // Get the local up direction of the firingPoint
        Vector3 localUp = playerTransform.transform.up;
        Vector3 newRotation;
        Vector3 newPosition = playerTransform.position + (playerTransform.forward * _shotPositionOffset);
        for (int i = 0; i < shotsAmount; i++)
        {
            // Apply the local rotation in the local Y-axis direction of the firingPoint
            newRotation = playerTransform.transform.eulerAngles + localUp * currentLineRotation;

            _poolingManagerSO.PoolProjectile(prefab, newPosition, Quaternion.Euler(newRotation), damage);

            currentLineRotation += angleBetweenLines;
        }
    }

    /* Shooting approach explanation
        Implement a cone shooting pattern
        Example with 3 projectile lines and 60° degree angle
        60/3 = 20° degrees between each projectile line, there has to be 1 line
        pointing in the direction of the cursor.

        Example with 7 projectile lines and 60° degree angle
        60/7 = 8.57..° degrees between each projectile line, there has to be 1 line
        pointing in the direction of the cursor.

        Example with 12 projectile lines and 60° degree angle
        60/12 = 5° degrees between each projectile line, there has to be 1 line
        pointing in the direction of the cursor.

        The 'projectileLines - 1' fix is to evenly distribute the lines along the cone
        Without fix example: 60/3 = 20
        coneHalf = _coneDegree/2f = 60/2 = 30
        initialLineRotation = (-coneHalf) = -30
        -30° (First projectile spawn)
        -30° + 20° = -10° (Second projectile spawn) !!! The middle projectile should always be at 0°
        -10° + 20° = 10°(Third projectile spawn) !!! The final projectile Y rotation
        should be 30°
        -------------------------------------------------------------------
        Now with the fix:
        With fix example: 60/(3-1) = 30
        coneHalf = _coneDegree/2f = 60/2 = 30
        initialLineRotation = (-coneHalf) = -30
        -30° (First projectile spawn)
        -30° + 30° = 0° (Second projectile spawn)
        -10° + 30° = 30°(Third projectile spawn)
    */
}
