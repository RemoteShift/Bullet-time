using UnityEngine;

public class CameraLook : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float mouseSensitivity = 1f;
    [SerializeField] private Transform targetTransform;

    private float _xRotation;
    private Vector2 _mouseInput;
    private Vector2 _lookInput;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnEnable()
    {
        InputHandler.Instance.OnMouseInputChanged += HandleMouseInput;
    }

    private void OnDisable()
    {
        InputHandler.Instance.OnMouseInputChanged -= HandleMouseInput;
    }

    private void HandleMouseInput(Vector2 mouseInput)
    {
        _mouseInput = mouseInput;
    }

    private void Update()
    {
        _lookInput.x = _mouseInput.x * mouseSensitivity;
        _lookInput.y = _mouseInput.y * mouseSensitivity;

        _xRotation -= _lookInput.y;
        _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);
        transform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
        
        targetTransform.Rotate(Vector3.up * _lookInput.x);
    }
}
