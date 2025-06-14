using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Manages the construction menu UI and interaction modes.
/// Controls animation, toggling UI elements, and delegates building/tile placement commands.
/// </summary>
public class ConstructionMenuManager : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Animator controlling open/close animations for the construction menu.")]
    private Animator m_Animator;

    [SerializeField]
    [Tooltip("Button to open or close the construction menu.")]
    private GameObject m_openCloseBtn;

    [SerializeField]
    [Tooltip("UI element shown when building mode is active, allowing to stop building.")]
    private GameObject m_stopBuilding;

    /// <summary>
    /// Event invoked when entering or exiting construction mode.
    /// Parameter: true if in construction mode, false otherwise.
    /// </summary>
    public UnityEvent<bool> InConstructionMode = new UnityEvent<bool>();

    /// <summary>
    /// Singleton instance of the ConstructionMenuManager.
    /// </summary>
    public static ConstructionMenuManager Instance;

    [Tooltip("Parent GameObject holding the construction menu UI.")]
    public GameObject MenuHolder;

    private BuildingPlacementManager m_PlacementManager;

    private bool m_construction = false;
    private bool m_isOpen = false;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// Enforces the singleton pattern to prevent duplicates.
    /// </summary>
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }
    }

    /// <summary>
    /// Registers the listener for construction mode changes.
    /// </summary>
    private void Start()
    {
        InConstructionMode.AddListener(OnConstruction);
    }

    /// <summary>
    /// Toggles the construction menu open/close state with animation.
    /// </summary>
    public void OpenMenu()
    {
        m_isOpen = !m_isOpen;

        if (m_isOpen)
        {
            m_Animator.CrossFadeInFixedTime("Open", 0.3f);
        }
        else
        {
            m_Animator.CrossFadeInFixedTime("Close", 0.3f);
        }
    }

    /// <summary>
    /// Called when the construction mode changes.
    /// Updates UI button visibility accordingly.
    /// </summary>
    /// <param name="state">True if entering construction mode, false if exiting.</param>
    private void OnConstruction(bool state)
    {
        m_construction = state;

        m_openCloseBtn.SetActive(!m_construction);
        m_stopBuilding.SetActive(m_construction);
    }

    /// <summary>
    /// Switches to the next menu via the given ChangeMenu instance.
    /// </summary>
    /// <param name="changeMenu">ChangeMenu handler to switch menus.</param>
    public void Next(ChangeMenu changeMenu)
    {
        changeMenu.DoChangeMenu();
    }

    /// <summary>
    /// Starts placing a building.
    /// </summary>
    /// <param name="building">Building prefab or data to place.</param>
    public void StartPlacingBuilding(Building building)
    {
        m_PlacementManager?.StartPlacingBuilding(building);
    }

    /// <summary>
    /// Starts placing a tile.
    /// </summary>
    /// <param name="tile">Tile data to place.</param>
    public void StartPlacingTile(TileChange tile)
    {
        m_PlacementManager?.StartPlacingTile(tile);
    }

    /// <summary>
    /// Stops any ongoing building placement.
    /// </summary>
    public void StopBuilding()
    {
        m_PlacementManager?.StopBuilding();
    }

    /// <summary>
    /// Starts destruction mode (removing buildings or tiles).
    /// </summary>
    public void StartDestructionMode()
    {
        m_PlacementManager?.StartDestructionMode();
    }

    /// <summary>
    /// Stops destruction mode.
    /// </summary>
    public void StopDestructionMode()
    {
        m_PlacementManager?.StopDestructionMode();
    }

    /// <summary>
    /// Assigns the building placement manager to delegate placement and destruction actions.
    /// </summary>
    /// <param name="manager">The BuildingPlacementManager instance.</param>
    public void SetPlacementManager(BuildingPlacementManager manager)
    {
        m_PlacementManager = manager;
    }
}
