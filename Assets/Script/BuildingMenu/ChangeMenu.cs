using UnityEngine;

/// <summary>
/// Handles switching between UI menus by activating the next menu and deactivating the current one.
/// </summary>
public class ChangeMenu : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The GameObject of the menu to switch to when this menu is changed.")]
    private GameObject NextMenu;

    /// <summary>
    /// Activates the next menu and deactivates the current one.
    /// </summary>
    public void DoChangeMenu()
    {
        if (NextMenu == null)
        {
            Debug.LogWarning($"NextMenu is not assigned in {gameObject.name}");
            return;
        }

        NextMenu.SetActive(true);
        gameObject.SetActive(false);
    }
}
