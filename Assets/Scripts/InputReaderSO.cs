using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

/// <summary>
/// This serves as an intermediary object between the InputActions and the GameObjects receiving input.
/// GameObjects listen for UnityActions instead of subscribing to InputSystem events directly.
/// </summary>
[CreateAssetMenu(menuName = "Input Reader", fileName = "Input Reader")]
public class InputReaderSO : ScriptableObject, GameInputs.IPlayerActions
{
    [TextArea(5, 20)]
    [SerializeField] private string _description;

    private GameInputs _gameInputs;
    private InputAction _move;
    private InputAction _look;
    private InputAction _fire;

    public event UnityAction<Vector2> OnMoved = delegate { };
    public event UnityAction<Vector2> OnLooked = delegate { };
    public event UnityAction<bool> OnFired = delegate { };

    // Subscribe to events each time the object is loaded.
    private void OnEnable()
    {
        _gameInputs = new GameInputs();
        _gameInputs.Player.Enable();
        _gameInputs.Player.SetCallbacks(this);

        _move = _gameInputs.Player.Move;
        _look = _gameInputs.Player.Look;
        _fire = _gameInputs.Player.Fire;

        _move.performed += OnMove;
        _look.performed += OnLook;
        _fire.performed += OnFire;
    }

    // Unsubscribes from events to prevent errors.
    private void OnDisable()
    {
        _move.performed -= OnMove;
        _look.performed -= OnLook;
        _fire.performed -= OnFire;

        _gameInputs = null;
    }

    public void EnableInputActions() => _gameInputs.Player.Enable();
    public void DisableInputActions() => _gameInputs.Player.Disable();

    // Event handling methods

    // Implements the interface from auto-generated GameInputs class.
    // Invoke the OnMoved event when receiving input from Player.
    public void OnMove(InputAction.CallbackContext context)
    {
        if (_move.WasPerformedThisFrame())
        {
            OnMoved?.Invoke(context.ReadValue<Vector2>());
        }
        else
        {
            OnMoved?.Invoke(Vector2.zero);
        }
    }

    // Implements the interface from auto-generated GameInputs class.
    // Invoke the OnLook event when receiving input from Player.
    public void OnLook(InputAction.CallbackContext context)
    {
        if (_look.WasPerformedThisFrame())
        {
            OnLooked?.Invoke(context.ReadValue<Vector2>());
        }
        else
        {
            OnLooked?.Invoke(Vector2.zero);
        }
    }

    // Implements the interface from auto-generated GameInputs class.
    // Invoke the OnFire event when action is performed.
    public void OnFire(InputAction.CallbackContext context)
    {
        if (_fire.WasPerformedThisFrame())
        {
            OnFired?.Invoke(true);
        }
        else
        {
            OnFired?.Invoke(false);
        }
    }
}
