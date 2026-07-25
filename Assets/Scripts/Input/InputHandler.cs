using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class InputHandler : Singleton<InputHandler>
{
    [FormerlySerializedAs("MouseInput")]
    [FormerlySerializedAs("mouseInput")]
    [Header("Action References")] 
    [SerializeField] private InputActionReference LookInput;
    [SerializeField] private InputActionReference LeftClickInput;
    [SerializeField] private InputActionReference MoveInput;
    [SerializeField] private InputActionReference JumpInput;
    [SerializeField] private InputActionReference DashInput;
    [SerializeField] private InputActionReference SlideInput;
    [SerializeField] private InputActionReference InteractInput;
    [SerializeField] private InputActionReference ScrollWheelInput;
    
    public event Action<Vector2, bool> OnLookInputChanged;
    public event Action<bool> OnLeftClickInput;
    public event Action OnInteractInput;
    public event Action<int> OnScrollWheelRoll;
    
    public Vector2 moveInput { get; private set; }
    public bool JumpPressed { get; private set; }
    public bool SlidePressed { get; private set; }
    public bool IsHoldingSlide { get; private set; } // Track continuous press state
    public bool DashPressed { get; private set; }

    private void OnEnable()
    {
        LookInput.action.Enable();
        LeftClickInput.action.Enable();
        MoveInput.action.Enable();
        JumpInput.action.Enable();
        DashInput.action.Enable();
        SlideInput.action.Enable();
        InteractInput.action.Enable();
        ScrollWheelInput.action.Enable();

        LookInput.action.performed += HandleLookInput;
        LookInput.action.canceled += HandleLookInput;
        LeftClickInput.action.performed += HandleLeftClickInput;
        LeftClickInput.action.canceled += HandleLeftClickInput;
        InteractInput.action.performed += HandleInteractInput;
        ScrollWheelInput.action.performed += HandleScrollWheelInput;
    }

    private void OnDisable()
    {
        LookInput.action.performed -= HandleLookInput;
        LookInput.action.canceled -= HandleLookInput;
        LeftClickInput.action.performed -= HandleLeftClickInput;
        LeftClickInput.action.canceled -= HandleLeftClickInput;
        InteractInput.action.performed -= HandleInteractInput;
        ScrollWheelInput.action.performed -= HandleScrollWheelInput;

        LookInput.action.Disable();
        LeftClickInput.action.Disable();
        MoveInput.action.Disable();
        JumpInput.action.Disable();
        DashInput.action.Disable();
        SlideInput.action.Disable();
        InteractInput.action.Disable();
        ScrollWheelInput.action.Disable();
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

    private void HandleLookInput(InputAction.CallbackContext context)
    {
        OnLookInputChanged?.Invoke(context.ReadValue<Vector2>(), context.control.device is Gamepad);
    }

    private void HandleLeftClickInput(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            OnLeftClickInput?.Invoke(true);
        }
        else if(context.canceled)
        {
            OnLeftClickInput?.Invoke(false);
        }
    }

    private void HandleInteractInput(InputAction.CallbackContext context)
    {
        OnInteractInput?.Invoke();
    }

    private void HandleScrollWheelInput(InputAction.CallbackContext context)
    {
        OnScrollWheelRoll?.Invoke((int)context.ReadValue<Vector2>().y);
    }
}