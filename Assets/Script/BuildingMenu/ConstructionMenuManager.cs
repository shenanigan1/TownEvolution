using UnityEngine;
using UnityEngine.Events;

public class ConstructionMenuManager : MonoBehaviour
{
    [SerializeField] private Animator m_Animator;
    [SerializeField] private GameObject m_openCloseBtn;
    [SerializeField] private GameObject m_stopBuilding;
    public UnityEvent<bool> InConstructionMode = new UnityEvent<bool>();
    public static ConstructionMenuManager Instance;
    public GameObject MenuHolder;

    private BuildingPlacementManager m_PlacementManager;
    private bool m_construction = false;
    private bool m_isOpen = false;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }
    private void Start()
    {
        InConstructionMode.AddListener(OnConstruction);
    }
    public void OpenMenu()
    {
        m_isOpen = !m_isOpen;
        if(m_isOpen)
        {
            m_Animator.CrossFadeInFixedTime("Open", 0.3f);
            return;
        }
        m_Animator.CrossFadeInFixedTime("Close", 0.3f);
    }

    private void OnConstruction(bool state)
    {
        m_construction = state;

        m_openCloseBtn.SetActive(!m_construction);
        m_stopBuilding.SetActive(m_construction);
    }

    public void Next(ChangeMenu changeMenu)
    {
        changeMenu.DoChangeMenu();
    }

    public void StartPlacingBuilding(Building building)
    {
        m_PlacementManager.StartPlacingBuilding(building);
    }

    public void StartPlacingTile(TileChange tile)
    {
        m_PlacementManager.StartPlacingTile(tile);
    }

    public void StopBuilding()
    {
        m_PlacementManager.StopBuilding();
    }

    public void StartDestructionMode()
    {
        m_PlacementManager.StartDestructionMode();
    }

    public void StopDestructionMode()
    {
        m_PlacementManager.StopDestructionMode();
    }
    public void SetPlacementManager(BuildingPlacementManager manager)
    {
        m_PlacementManager = manager;
    }
}
