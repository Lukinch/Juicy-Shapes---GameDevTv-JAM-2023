using NaughtyAttributes;
using UnityEngine;

/// <summary>
/// Class in charge of moving the character based on inputs.
/// </summary>
public class PlayerMovement : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField, Tooltip("Scriptable Object from which each input is read")]
    private InputReaderSO _inputReaderSO;
    [SerializeField, Tooltip("Unity Component 'Character Controller'")]
    private CharacterController _characterController;
    [SerializeField, Tooltip("Material with no friction")]
    private PhysicMaterial _physicMaterial;

    [Header("Configuration")]
    [SerializeField, Expandable]
    private PlayerMovementStatsSO _playerMovementStats;

    // player
    private float _speed;
    private float _targetSpeed;
    private float _targetRotation = 0.0f;

    // Stats
    private float _moveSpeed;
    private float _speedChangeRate;

    private Vector2 _moveVector;
    private Vector3 _inputDirection;

    public float MoveSpeed
    {
        get => _moveSpeed;
        set => _moveSpeed = value;
    }
    public float Acceleration
    {
        get => _speedChangeRate;
        set => _speedChangeRate = value;
    }

    private void OnEnable()
    {
        _moveSpeed = _playerMovementStats.MoveSpeed;
        _speedChangeRate = _playerMovementStats.SpeedChangeRate;
        _characterController.material = _physicMaterial;

        _inputReaderSO.OnMoved += Input_OnMoved;
    }
    private void OnDisable()
    {
        _inputReaderSO.OnMoved -= Input_OnMoved;
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        // targetSpeed is left in purpose for a case in the future in which we will have different target speeds
        // base on different inputs or actions
        _targetSpeed = _moveSpeed;

        // a simplistic acceleration and deceleration designed to be easy to remove, replace, or iterate upon

        // note: Vector2's == operator uses approximation so is not floating point error prone, and is cheaper than magnitude
        // if there is no input, set the target speed to 0
        if (_moveVector == Vector2.zero) _targetSpeed = 0.0f;

        // a reference to the players current horizontal velocity
        float currentHorizontalSpeed = new Vector3(_characterController.velocity.x, 0.0f, _characterController.velocity.z).magnitude;

        float speedOffset = 0.1f;

        // accelerate or decelerate to target speed
        if (currentHorizontalSpeed < _targetSpeed - speedOffset ||
            currentHorizontalSpeed > _targetSpeed + speedOffset)
        {
            // creates curved result rather than a linear one giving a more organic speed change
            // note T in Lerp is clamped, so we don't need to clamp our speed
            _speed = Mathf.Lerp(currentHorizontalSpeed, _targetSpeed,
                Time.deltaTime * _speedChangeRate);

            // round speed to 3 decimal places
            _speed = Mathf.Round(_speed * 1000f) / 1000f;
        }
        else
        {
            _speed = _targetSpeed;
        }

        // note: Vector2's != operator uses approximation so is not floating point error prone, and is cheaper than magnitude
        // if there is a move input rotate player when the player is moving
        if (_moveVector != Vector2.zero)
        {
            _targetRotation = Mathf.Atan2(_inputDirection.x, _inputDirection.z) * Mathf.Rad2Deg;
        }


        Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;

        // move the player
        _characterController.Move(targetDirection.normalized * (_speed * Time.deltaTime));
    }

    private void Input_OnMoved(Vector2 moveVector)
    {
        _moveVector = moveVector;
        _inputDirection = new(_moveVector.x, 0.0f, _moveVector.y);
    }

    public void StopMoving()
    {
        _moveVector = Vector2.zero;
        _inputDirection = Vector3.zero;
    }
}
