using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : Singleton<InputHandler>
{
    [Header("Action References")] 
    [SerializeField] private InputActionReference mouseInput;
    
    public event Action<Vector2> OnMouseInputChanged;

    private void OnEnable()
    {
        mouseInput.action.Enable();
        
        mouseInput.action.performed += HandleMouseInput;
        mouseInput.action.canceled += HandleMouseInput;
    }

    private void OnDisable()
    {
        mouseInput.action.performed -= HandleMouseInput;
        mouseInput.action.canceled -= HandleMouseInput;
        
        mouseInput.action.Disable();
    }

    private void HandleMouseInput(InputAction.CallbackContext context)
    {
        OnMouseInputChanged?.Invoke(context.ReadValue<Vector2>());
    }
}
