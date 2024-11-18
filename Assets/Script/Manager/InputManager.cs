using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;
    public UnityEvent<float> Zooming = new();
    public UnityEvent<Vector2> Moving = new();
    public UnityEvent<bool> LeftMouseBtnPressed = new();
    public UnityEvent<bool> RightMouseBtnPressed = new();
    public UnityEvent<Vector2> MousePositionChange = new();

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            return;
        }
        Destroy(this);
    }

    public void OnZoom(InputAction.CallbackContext context)
    {
        Zooming.Invoke(-Mathf.Clamp(context.ReadValue<float>(), -1, 1));
    }

    public void OnMouseMove(InputAction.CallbackContext context)
    {
            Moving.Invoke(context.ReadValue<Vector2>());
    }

    public void MouseLeftBtnPress(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            LeftMouseBtnPressed.Invoke(true);
        }
        else if (context.canceled)
        {
            LeftMouseBtnPressed.Invoke(false);
        }
    }

    public void MouseRightBtnPress(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            RightMouseBtnPressed.Invoke(true);
        }
        else if (context.canceled)
        {
            RightMouseBtnPressed.Invoke(false);
        }
    }

    public void MousePosition(InputAction.CallbackContext context)
    {
        MousePositionChange.Invoke(context.ReadValue<Vector2>());
    }
}
