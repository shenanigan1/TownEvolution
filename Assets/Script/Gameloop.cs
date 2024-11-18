using UnityEngine;

public class Gameloop : MonoBehaviour
{
    public static Gameloop Instance;
    private Vector2Int lastChange = new Vector2Int(-1, -1);
    private SquareColorStrategy m_colorStrategy;
    private Selection m_selection;
    [SerializeField] private BuildingPlacementParams m_buildingPlacementParams;
    private BuildingPlacementManager m_buildingPlacementManager;
    private ChunckManager m_chunckManager;
    private bool isPlaying = false;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);

        m_selection = new Selection(ECaseType.Selection, ECaseType.BadSelection);
        m_buildingPlacementManager = gameObject.AddComponent<BuildingPlacementManager>();
        m_buildingPlacementParams.selection = m_selection;
        m_buildingPlacementManager.Init(m_buildingPlacementParams);
        
    }

    public void Init()
    {
        ConstructionMenuManager.Instance.SetPlacementManager(m_buildingPlacementManager);
        m_chunckManager = ChunckManager.Instance;
        isPlaying = true;
    }

    void Update()
    {
        if (isPlaying)
            MouseInputHandler.ProcessMouseInput(m_colorStrategy, ref lastChange, m_chunckManager.GetGridSize, m_selection.GetSelectionTile());
    }

    public void SetColorStrategie(SquareColorStrategy strategy)
    { m_colorStrategy = strategy; }
}
