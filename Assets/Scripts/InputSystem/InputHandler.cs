using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    public static event System.Action<Vector2> OnTouch;
    public static event System.Action<Vector2> OnDelta;

    private PlayerInputActions _playerInputActions;

    private InputAction _touch;
    private InputAction _delta;

    private void Awake()
    {
        _playerInputActions = new PlayerInputActions();

        _touch = _playerInputActions.ActionMap.Touch;
        _delta = _playerInputActions.ActionMap.Delta;

        _touch.performed += HandleTouch;
        _delta.performed += HandleDelta;
    }

    private void OnEnable()
    {
        _touch.Enable();
        _delta.Enable();
    }

    private void OnDisable()
    {
        _touch.Disable();
        _delta.Disable();
    }

    private void HandleTouch(InputAction.CallbackContext context)
    {
        Vector2 mousePosition = _touch.ReadValue<Vector2>();

        OnTouch?.Invoke(mousePosition);
    }

    private void HandleDelta(InputAction.CallbackContext context)
    {
        Debug.Log(_delta.ReadValue<Vector2>());
        if (_delta.ReadValue<Vector2>().sqrMagnitude < 250f) return;

        Vector2 mouseDelta = _delta.ReadValue<Vector2>();

        OnDelta?.Invoke(mouseDelta);
    }
}
