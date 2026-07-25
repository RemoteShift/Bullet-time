using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class InputHandler : Singleton<InputHandler>
{
    [FormerlySerializedAs("mouseInput")]
    [Header("Action References")] 
    [SerializeField] private InputActionReference MouseInput;
    [SerializeField] private InputActionReference LeftClickInput;
    [SerializeField] private InputActionReference MoveInput;
    [SerializeField] private InputActionReference JumpInput;
    [SerializeField] private InputActionReference DashInput;
    [SerializeField] private InputActionReference SlideInput;
    
    public event Action<Vector2> OnMouseInputChanged;
    public event Action<bool> OnLeftClickInput;
    
    public Vector2 moveInput { get; private set; }
    public bool JumpPressed { get; private set; }
    public bool SlidePressed { get; private set; }
    public bool IsHoldingSlide { get; private set; } // Track continuous press state
    public bool DashPressed { get; private set; }

    private void OnEnable()
    {
        MouseInput.action.Enable();
        LeftClickInput.action.Enable();
        MoveInput.action.Enable();
        JumpInput.action.Enable();
        DashInput.action.Enable();
        SlideInput.action.Enable();

        MouseInput.action.performed += HandleMouseInput;
        MouseInput.action.canceled += HandleMouseInput;
        LeftClickInput.action.performed += HandleLeftClickInput;
        LeftClickInput.action.canceled += HandleLeftClickInput;
    }

    private void OnDisable()
    {
        MouseInput.action.performed -= HandleMouseInput;
        MouseInput.action.canceled -= HandleMouseInput;
        LeftClickInput.action.performed -= HandleLeftClickInput;
        LeftClickInput.action.canceled -= HandleLeftClickInput;

        MouseInput.action.Disable();
        LeftClickInput.action.Disable();
        MoveInput.action.Disable();
        JumpInput.action.Disable();
        DashInput.action.Disable();
        SlideInput.action.Disable();
    }

    private void Update()
    {
        moveInput = MoveInput.action.ReadValue<Vector2>();
        
        JumpPressed  = JumpInput.action.WasPressedThisFrame();
        DashPressed  = DashInput.action.WasPressedThisFrame();
        SlidePressed = SlideInput.action.WasPressedThisFrame();
        
        // Is user holding the key right now?
        IsHoldingSlide = SlideInput.action.IsPressed(); 
    }

    private void HandleMouseInput(InputAction.CallbackContext context)
    {
        OnMouseInputChanged?.Invoke(context.ReadValue<Vector2>());
    }

    private void HandleLeftClickInput(InputAction.CallbackContext context)
    {
        Debug.Log("Left clicked");
        if(context.performed)
        {
            OnLeftClickInput?.Invoke(true);
        }
        else if(context.canceled)
        {
            OnLeftClickInput?.Invoke(false);
        }
    }
}