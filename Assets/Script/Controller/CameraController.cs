using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Controls a 2D orthographic camera with zoom and movement locked to right mouse button.
/// Uses InputManager events to receive input.
/// </summary>
public class CameraController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Camera m_camera;
    [SerializeField] private Transform m_cameraTransform;

    [Header("Movement Parameters")]
    [SerializeField] private float m_maxSizeZoom = 10f;
    [SerializeField] private float m_minSizeZoom = 2f;
    [SerializeField] private float m_zoomSpeed = 1f;
    [SerializeField] private float m_moveSpeed = 10f;

    private Vector2 m_mousePosition;
    private bool m_isWannaMove;

    private void Start()
    {
        if (m_camera == null)
        {
            Debug.LogError("Camera reference not set in CameraController.");
            enabled = false;
            return;
        }

        m_cameraTransform = m_camera.transform;

        var inputManager = InputManager.Instance;
        inputManager.Moving.AddListener(OnMove);
        inputManager.Zooming.AddListener(OnZoom);
        inputManager.RightMouseBtnPressed.AddListener(SetIsWannamove);
    }

    /// <summary>
    /// Called when right mouse button pressed/released to enable or disable camera movement.
    /// Locks cursor when moving and saves initial mouse position.
    /// </summary>
    public void SetIsWannamove(bool value)
    {
        m_isWannaMove = value;

        if (m_isWannaMove)
            m_mousePosition = Mouse.current.position.ReadValue();

        Cursor.lockState = m_isWannaMove ? CursorLockMode.Locked : CursorLockMode.None;

        // Warp cursor back to saved position to avoid cursor drifting.
        Mouse.current.WarpCursorPosition(m_mousePosition);
    }

    /// <summary>
    /// Called on zoom input (scroll wheel).
    /// Zooms the orthographic camera size clamped between min and max.
    /// </summary>
    public void OnZoom(float value)
    {
        m_camera.orthographicSize = Mathf.Clamp(m_camera.orthographicSize + (value * m_zoomSpeed), m_minSizeZoom, m_maxSizeZoom);
    }

    /// <summary>
    /// Called on move input (mouse delta).
    /// Moves the camera only if right mouse button is held.
    /// </summary>
    public void OnMove(Vector2 position)
    {
        if (!m_isWannaMove)
            return;

        // Keep cursor locked in the initial position
        Mouse.current.WarpCursorPosition(m_mousePosition);

        // Convert 2D input vector to 3D world direction (x, 0, y)
        Vector3 inputDirection = new Vector3(position.x, 0, position.y);

        // Transform input direction to camera's local space
        Vector3 cameraDirection = m_cameraTransform.TransformVector(inputDirection);

        // Move the camera horizontally (x and z), keep y position fixed
        m_cameraTransform.position += new Vector3(cameraDirection.x, 0, cameraDirection.z) * m_moveSpeed * Time.deltaTime;
    }
}
