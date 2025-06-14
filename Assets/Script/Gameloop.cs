using UnityEngine;

/// <summary>
/// Singleton managing the main game loop and core gameplay logic.
/// Handles building placement and grid interactions.
/// </summary>
public class Gameloop : MonoBehaviour
{
    public static Gameloop Instance { get; private set; }  // Singleton instance with private setter for safety

    private Vector2Int lastChange = new Vector2Int(-1, -1);
    private SquareColorStrategy m_colorStrategy;
    private Selection m_selection;
    [SerializeField] private BuildingPlacementParams m_buildingPlacementParams;

    private BuildingPlacementManager m_buildingPlacementManager;
    private ChunckManager m_chunckManager;
    private bool isPlaying = false;

    /// <summary>
    /// Initialize singleton instance and setup dependencies.
    /// </summary>
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Optional: persist across scenes
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instances
            return;
        }

        m_selection = new Selection(ECaseType.Selection, ECaseType.BadSelection);
        m_buildingPlacementManager = gameObject.AddComponent<BuildingPlacementManager>();
        m_buildingPlacementParams.selection = m_selection;
        m_buildingPlacementManager.Init(m_buildingPlacementParams);
    }

    /// <summary>
    /// Called externally to start the gameplay loop.
    /// </summary>
    public void Init()
    {
        ConstructionMenuManager.Instance.SetPlacementManager(m_buildingPlacementManager);
        m_chunckManager = ChunckManager.Instance;
        isPlaying = true;
    }

    /// <summary>
    /// Update is called once per frame.
    /// Processes mouse input if the game is playing.
    /// </summary>
    private void Update()
    {
        if (!isPlaying) return;

        // Process mouse input with the current color strategy, last changed position, chunk grid size, and current selection tile.
        MouseInputHandler.ProcessMouseInput(
            m_colorStrategy,
            ref lastChange,
            m_chunckManager.GetGridSize,
            m_selection.GetSelectionTile());
    }

    /// <summary>
    /// Set the color strategy to use for grid/square coloring.
    /// </summary>
    /// <param name="strategy">The new square color strategy.</param>
    public void SetColorStrategie(SquareColorStrategy strategy)
    {
        m_colorStrategy = strategy;
    }
}
