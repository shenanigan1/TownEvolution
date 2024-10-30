using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gameloop : MonoBehaviour
{
    public static Gameloop Instance;
    private Vector2Int lastChange = new Vector2Int(-1, -1);
    private SquareColorStrategy m_colorStrategy;
    private Selection m_selection;
    [SerializeField] private BuildingPlacementParams m_buildingPlacementParams;
    private BuildingPlacementManager m_buildingPlacementManager;

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

    private void Start()
    {
        ConstructionMenuManager.Instance.SetPlacementManager(m_buildingPlacementManager);
    }

    void Update()
    {
        MouseInputHandler.ProcessMouseInput(m_colorStrategy, ref lastChange, ChunckManager.Instance.GetGridSize, m_selection.GetSelectionTile());
        m_buildingPlacementManager.Update();
    }

    public void SetColorStrategie(SquareColorStrategy strategy)
    { m_colorStrategy = strategy; }
}
