using NaughtyAttributes;
using UnityEngine;

/// <summary>
/// Class in charge of rotating the character's visuals based on inputs.
/// </summary>
public class PlayerLook : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField, Tooltip("Scriptable Object from which each input is read")]
    private InputReaderSO _inputReaderSO;
    [SerializeField, Tooltip("The object containing the visuals to rotate")]
    private Transform _visualsToRotate;

    [Header("Configuration")]
    [SerializeField, Expandable]
    private PlayerMovementStatsSO _playerMovementStats;
    [SerializeField] private LayerMask _mouseLook;

    private Camera _mainCamera;
    private Ray _ray;
    private Vector2 _mousePositionOnScreen;
    private Vector3 _mousePositionInWorld;
    private Vector3 _lookPosition;
    private Vector3 _lookDirection;
    private float _targetRotation;
    private float _rotationVelocity;
    private float _smoothRotation;

    void Awake() => _mainCamera = Camera.main;

    private void OnEnable() => _inputReaderSO.OnLooked += Input_OnLooked;
    private void OnDisable() => _inputReaderSO.OnLooked -= Input_OnLooked;

    private void Update()
    {
        _mousePositionInWorld = GetMousePositionInWorld();

        _lookPosition = new(_mousePositionInWorld.x, _visualsToRotate.position.y, _mousePositionInWorld.z);
        _lookDirection = (_lookPosition - _visualsToRotate.position).normalized;

        _targetRotation = Mathf.Atan2(_lookDirection.x, _lookDirection.z) * Mathf.Rad2Deg;
        _smoothRotation = Mathf.SmoothDampAngle(_visualsToRotate.eulerAngles.y, _targetRotation, ref _rotationVelocity,
            _playerMovementStats.RotationSmoothTime);

        _visualsToRotate.rotation = Quaternion.Euler(0.0f, _smoothRotation, 0.0f);
    }

    private Vector3 GetMousePositionInWorld()
    {
        // Cast a ray from the position of the pointer on the screen forward
        _ray = _mainCamera.ScreenPointToRay(_mousePositionOnScreen);
        // Detect ray collision with specified layer mask
        Physics.Raycast(_ray, out RaycastHit raycastHit, float.MaxValue, _mouseLook);
        return raycastHit.point;
    }

    private void Input_OnLooked(Vector2 mousePositionOnScreen)
    {
        if (mousePositionOnScreen != Vector2.zero)
            _mousePositionOnScreen = mousePositionOnScreen;
    }
}
