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
    private InputAction _deltaPosition;
    private Vector2 _firstTouchPosition;
    private Vector2 _lastTouchPosition;
    private bool _deltaThresholdPassed;

    private void Awake()
    {
        _playerInputActions = new PlayerInputActions();

        _touch = _playerInputActions.ActionMap.Touch;
        _delta = _playerInputActions.ActionMap.Delta;
        _deltaPosition = _playerInputActions.ActionMap.DeltaPosition;

        _touch.performed += HandleTouch;
        _delta.performed += HandleDelta;
        _deltaPosition.performed += HandleDeltaPosition;
    }

    private void OnEnable()
    {
        _touch.Enable();
        _delta.Enable();
        _deltaPosition.Enable();
    }

    private void OnDisable()
    {
        _touch.Disable();
        _delta.Disable();
        _deltaPosition.Disable();
    }

    private void HandleTouch(InputAction.CallbackContext context)
    {
        _firstTouchPosition = _touch.ReadValue<Vector2>();

        OnTouch?.Invoke(_firstTouchPosition);
    }

    private void HandleDelta(InputAction.CallbackContext context)
    {
        Vector2 mouseDelta = _delta.ReadValue<Vector2>();

        if (mouseDelta.sqrMagnitude < 100f) return;

        _deltaThresholdPassed = true;
    }

    private void HandleDeltaPosition(InputAction.CallbackContext context)
    {
        if (_deltaThresholdPassed == false) return;

        _lastTouchPosition = _deltaPosition.ReadValue<Vector2>();
        Vector2 pullDirection = _lastTouchPosition - _firstTouchPosition;

        OnDelta?.Invoke(pullDirection);

        _deltaThresholdPassed = false;
    }
}
