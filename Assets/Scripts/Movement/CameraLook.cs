using UnityEngine;

public class CameraLook : Singleton<CameraLook>
{
    [SerializeField] private float mouseSensitivity = 1f;
    [SerializeField] private float controllerLookSensitivity = 1f;
    [SerializeField] private Transform yawPivot;
    [SerializeField] private Transform pitchPivot;

    private float yaw;
    private float pitch;
    private Vector2 lookInput;
    private bool isUsingGamepad;

    private void OnEnable()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        // Pass the action context or device info from your InputHandler
        InputHandler.Instance.OnLookInputChanged += HandleLookInput;
    }

    private void OnDisable()
    {
        InputHandler.Instance.OnLookInputChanged -= HandleLookInput;
    }

    // Update your handler signature to accept a device flag (or check InputSystem directly)
    private void HandleLookInput(Vector2 input, bool isGamepad)
    {
        lookInput = input;
        isUsingGamepad = isGamepad;
    }

    private void Update()
    {
        // Pick sensitivity based on active device
        var currentSensitivity = isUsingGamepad ? controllerLookSensitivity : mouseSensitivity;

        // Frame-rate independence for controllers (mice already output delta per frame)
        if (isUsingGamepad)
        {
            yaw += lookInput.x * currentSensitivity * Time.deltaTime;
            pitch -= lookInput.y * currentSensitivity * Time.deltaTime;
        }
        else
        {
            yaw += lookInput.x * currentSensitivity;
            pitch -= lookInput.y * currentSensitivity;
        }

        pitch = Mathf.Clamp(pitch, -90f, 90f);

        yawPivot.localRotation = Quaternion.Euler(0f, yaw, 0f);
        pitchPivot.localRotation = Quaternion.Euler(pitch, 0f, 0f);
    }
}