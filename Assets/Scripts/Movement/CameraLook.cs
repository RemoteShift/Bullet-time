using UnityEngine;

public class CameraLook : Singleton<CameraLook>
{
    [SerializeField] private float mouseSensitivity = 1f;
    [SerializeField] private Transform yawPivot;
    [SerializeField] private Transform pitchPivot;

    private float yaw;
    private float pitch;
    private Vector2 mouseInput;

    private void OnEnable()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        InputHandler.Instance.OnMouseInputChanged += HandleMouseInput;
    }

    private void OnDisable()
    {
        InputHandler.Instance.OnMouseInputChanged -= HandleMouseInput;
    }

    private void HandleMouseInput(Vector2 input)
    {
        mouseInput = input;
    }

    private void Update()
    {
        yaw += mouseInput.x * mouseSensitivity;
        pitch -= mouseInput.y * mouseSensitivity;
        pitch = Mathf.Clamp(pitch, -90f, 90f);

        yawPivot.localRotation = Quaternion.Euler(0f, yaw, 0f);
        pitchPivot.localRotation = Quaternion.Euler(pitch, 0f, 0f);
    }
}