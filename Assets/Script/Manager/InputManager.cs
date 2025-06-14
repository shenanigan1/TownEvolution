using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

/// <summary>
/// Singleton InputManager using Unity's new InputSystem,
/// wraps input callbacks and dispatches events.
/// </summary>
public class InputManager : MonoBehaviour
{
    public static InputManager Instance;

    /// <summary> Event triggered on zoom input (mouse wheel). </summary>
    public UnityEvent<float> Zooming = new();

    /// <summary> Event triggered on mouse movement input. </summary>
    public UnityEvent<Vector2> Moving = new();

    /// <summary> Event triggered when left mouse button is pressed or released. </summary>
    public UnityEvent<bool> LeftMouseBtnPressed = new();

    /// <summary> Event triggered when right mouse button is pressed or released. </summary>
    public UnityEvent<bool> RightMouseBtnPressed = new();

    /// <summary> Event triggered on raw mouse position change. </summary>
    public UnityEvent<Vector2> MousePositionChange = new();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keep input manager persistent if desired
            return;
        }

        Destroy(gameObject); // Destroy duplicate instance
    }

    /// <summary>
    /// Called by InputSystem on zoom (mouse wheel scroll).
    /// Clamps input to -1..1 and inverts sign to match zoom direction.
    /// </summary>
    public void OnZoom(InputAction.CallbackContext context)
    {
        float zoomValue = context.ReadValue<float>();
        // Clamp zoom to [-1, 1] and invert sign to match desired zoom direction
        Zooming.Invoke(-Mathf.Clamp(zoomValue, -1f, 1f));
    }

    /// <summary>
    /// Called by InputSystem on mouse movement.
    /// Passes the raw mouse delta movement.
    /// </summary>
    public void OnMouseMove(InputAction.CallbackContext context)
    {
        Vector2 moveValue = context.ReadValue<Vector2>();
        Moving.Invoke(moveValue);
    }

    /// <summary>
    /// Called on left mouse button press/release.
    /// Invokes with true on press, false on release.
    /// </summary>
    public void MouseLeftBtnPress(InputAction.CallbackContext context)
    {
        if (context.started)
            LeftMouseBtnPressed.Invoke(true);
        else if (context.canceled)
            LeftMouseBtnPressed.Invoke(false);
    }

    /// <summary>
    /// Called on right mouse button press/release.
    /// Invokes with true on press, false on release.
    /// </summary>
    public void MouseRightBtnPress(InputAction.CallbackContext context)
    {
        if (context.started)
            RightMouseBtnPressed.Invoke(true);
        else if (context.canceled)
            RightMouseBtnPressed.Invoke(false);
    }

    /// <summary>
    /// Called on raw mouse position change.
    /// Useful if you want absolute cursor position.
    /// </summary>
    public void MousePosition(InputAction.CallbackContext context)
    {
        Vector2 mousePos = context.ReadValue<Vector2>();
        MousePositionChange.Invoke(mousePos);
    }
}
