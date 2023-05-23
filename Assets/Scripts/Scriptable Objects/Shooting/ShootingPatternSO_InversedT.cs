using System;
using System.Collections;
using System.Collections.Generic;
using GlobalEnums;
using UnityEngine;

/// <summary>
/// Scriptable Object in charge of firing projectiles in a inverted T fashion
/// </summary>
[CreateAssetMenu(
    menuName = "Project Juicer/Shooting Pattern/Inverted T",
    fileName = "Shooting Pattern Inverted T")]
public class ShootingPatternSO_InversesT : ShootingPatternSO
{
    [SerializeField, Range(0.001f, 1f)]
    [Tooltip("Distance between parallel bullets per shot")]
    private float _distanceBetweenLines = 0.2f;
    [SerializeField, Min(0.1f)]
    [Tooltip("The offset from the player center. Treat the minimum of 1 as the original position.")]
    private float _shotPositionOffset = 1.3f;
    [SerializeField] private float[] _rotationOffsets;

    public override void Fire(Transform playerTransform, GameObject prefab, int shotsAmount, float damage, ThemeColor themeColor)
    {
        //// Projectile lines should always be odd, so at least 1 bullet is shot in the direction of the cursor
        //// if (shotsAmount % 2 == 0) shotsAmount -= 1;
        // The 'shotsAmount - 1' is to evenly distribute the lines
        float linesMaxDistance = (shotsAmount - 1) * _distanceBetweenLines;

        // With the distance half I can start from -distanceHalf and add '_distanceBetweenLines'
        // in each 'for' iteration
        float distanceHalf = linesMaxDistance / 2f;

        float currentLinePosition = -distanceHalf;

        // Get the local right direction of the firingPoint
        Vector3 localRight = playerTransform.transform.right;
        Vector3 localForward = playerTransform.transform.forward;
        Vector3 newPosition;
        Vector3 offsetPosition;
        Vector3 newRotation;

        for (int i = 0; i < _rotationOffsets.Length; i++)
        {
            if (i == 0)
            {
                newPosition = playerTransform.position + localRight * -_shotPositionOffset;
            }
            else if (i == 1)
            {
                newPosition = playerTransform.position + localForward * _shotPositionOffset;
            }
            else
            {
                newPosition = playerTransform.position + localRight * _shotPositionOffset;
            }

            newRotation = playerTransform.eulerAngles;
            newRotation.y += _rotationOffsets[i];

            for (int j = 0; j < shotsAmount; j++)
            {
                if (i == 0)
                {
                    offsetPosition = newPosition + (localForward * currentLinePosition);
                }
                else if (i == 1)
                {
                    offsetPosition = newPosition + (localRight * currentLinePosition);
                }
                else
                {
                    offsetPosition = newPosition + (localForward * -currentLinePosition);
                }

                _poolingManagerSO.PoolProjectile(prefab, offsetPosition, Quaternion.Euler(newRotation), damage, themeColor);

                currentLinePosition += _distanceBetweenLines;
            }

            currentLinePosition = -distanceHalf;
        }
    }
}
