using UnityEngine;

[CreateAssetMenu(
    menuName = "Project Juicer/Player/Movement Stats",
    fileName = "Player Movement Stats")]
public class PlayerMovementStatsSO : ScriptableObject
{
    [Header("Movement Options")]
    [SerializeField, Min(0)]
    [Tooltip("Move speed of the character in m/s")]
    private float _moveSpeed = 5.335f;
    [SerializeField, Min(0)]
    [Tooltip("Acceleration and deceleration")]
    private float _speedChangeRate = 10.0f;
    [Header("Rotation Options")]
    [SerializeField, Range(0.0f, 0.5f)]
    [Tooltip("How fast the character turns to face movement direction")]
    private float _rotationSmoothTime = 0.08f;

    public float MoveSpeed => _moveSpeed;
    public float SpeedChangeRate => _speedChangeRate;
    public float RotationSmoothTime => _rotationSmoothTime;
}
