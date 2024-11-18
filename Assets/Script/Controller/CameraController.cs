using UnityEngine;
using UnityEngine.InputSystem;


public class CameraController : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] private Camera m_camera;
    [SerializeField] private Transform m_cameraTransform;

    [Header("Deplacement Parameters")]
    [SerializeField] private float m_maxSizeZoom;
    [SerializeField] private float m_minSizeZoom;
    [SerializeField] private float m_zoomSpeed;
    [SerializeField] private float m_moveSpeed;  

    private Vector2 m_mousePosition;
    private bool m_isWannaMove;

    private void Start()
    {
        InputManager inputManager = InputManager.Instance;
        m_cameraTransform = m_camera.transform;
        inputManager.Moving.AddListener(OnMove);
        inputManager.Zooming.AddListener(OnZoom);
        inputManager.RightMouseBtnPressed.AddListener(SetIsWannamove);
    }
    public void SetIsWannamove(bool value)
    {
        m_isWannaMove = value;
        if (m_isWannaMove)
            m_mousePosition = Mouse.current.position.value;
        Cursor.lockState = m_isWannaMove ? CursorLockMode.Locked : CursorLockMode.None;
        Mouse.current.WarpCursorPosition(m_mousePosition);
    }

    public void OnZoom(float value)
    {
        m_camera.orthographicSize = Mathf.Clamp(m_camera.orthographicSize+(value*m_zoomSpeed), m_minSizeZoom, m_maxSizeZoom);
    }

    public void OnMove(Vector2 position)
    {
        if (!m_isWannaMove)
            return;

        Mouse.current.WarpCursorPosition(m_mousePosition);
        Vector3 pos = new Vector3(position.x,0,position.y); // Transform Vector2 in Vector3
        Vector3 CameraDirectionVector = m_cameraTransform.TransformVector(pos); // Put the Vector in the right direction for face the forward vector of the camera
        m_cameraTransform.position = new Vector3(
                                                    m_cameraTransform.position.x + (CameraDirectionVector.x * m_moveSpeed * Time.deltaTime),
                                                    m_cameraTransform.position.y,
                                                    m_cameraTransform.position.z + (CameraDirectionVector.z * m_moveSpeed * Time.deltaTime)
                                                );
    }
}
