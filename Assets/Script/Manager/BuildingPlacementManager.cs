using System.Collections.Generic;
using UnityEngine;

public class BuildingPlacementManager : MonoBehaviour
{
    private bool m_placeBuilding = false;
    private bool m_destroyBuilding = false;
    private Dictionary<(int,int), Building> m_buildings = new Dictionary<(int,int), Building>();
    [SerializeField]private Building m_currentBuilding;

    [SerializeField] private GameObject _preview;
    [SerializeField] private SpriteRenderer _previewRenderer;
    // Start is called before the first frame update
    void Start()
    {
        _previewRenderer = _preview.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_placeBuilding)
        {
            PlaceBuildingOnMousePosOnMap();
        }
        else if (m_destroyBuilding)
        {
            DestroyBuilding();
        }
    }

    private void DestroyBuilding()
    {
        Vector2Int? position = GridPositionUtility.GetGridPosition(Input.mousePosition);

        if (position != null)
        {
            _preview.transform.position = new Vector3(position.Value.x * 10, 0, position.Value.y * 10);

            if (!IsOccupied(position.Value.x, position.Value.y))
                return;

            if (Input.GetKeyDown(KeyCode.W))
            {
                m_buildings[(position.Value.x, position.Value.y)].Destroy();
            }
        }
    }

    private void PlaceBuildingOnMousePosOnMap()
    {
        Vector2Int? position = GridPositionUtility.GetGridPosition(Input.mousePosition);

        if (position != null)
        {

            _preview.transform.position = new Vector3(position.Value.x * 10, 0, position.Value.y * 10);

            if (!CanPlaceNChangePreviewColor(position.Value.x, position.Value.y, m_currentBuilding.price))
                 return;

            if(Input.GetKeyDown(KeyCode.W))
            {
                CreateBuilding(position.Value.x, position.Value.y);
            }
        }
    }

    private bool CanPlaceNChangePreviewColor(int x, int y, int price)
    {
        if (IsOccupied(x, y) || !CanBuy(price))
        {
            if (_previewRenderer.color == Color.green)
                _previewRenderer.color = Color.red;
            return false;
        }
        else if (_previewRenderer.color == Color.red)
        {
            _previewRenderer.color = Color.green;
        }
        return true;
    }

    private bool IsOccupied(int x, int y)
    {
        if(m_buildings.ContainsKey((x,y)))
        {
            return m_buildings[(x, y)] != null;
        }
        return false;
    }

    private bool CanBuy(int price)
    {
        return price <= RessourcesManager.Instance.GetRessources<Gold>().quantity;
    }

    private void CreateBuilding(int x, int y)
    {
        m_buildings[(x,y)] = Instantiate(m_currentBuilding);
        m_buildings[(x, y)].Place(new Vector3(x * 10, 0, y * 10));
    }

    public void StartPlacingBuilding(Building building)
    {
        StopDestructionMode();
        _preview.SetActive(true);
        m_currentBuilding = building;
        _previewRenderer.sprite = building.buildingPrefab.GetComponent<SpriteRenderer>().sprite;
        m_placeBuilding = true;
        ConstructionMenuManager.Instance.InConstructionMode.Invoke(true);
    }

    public void StopBuilding()
    {
        StopDestructionMode();
        _preview.SetActive(false);
        m_placeBuilding = false;
        ConstructionMenuManager.Instance.InConstructionMode.Invoke(false);
    }

    public void StartDestructionMode()
    {
        StopBuilding();
        m_destroyBuilding = true;
        ConstructionMenuManager.Instance.InConstructionMode.Invoke(true);
    }

    public void StopDestructionMode()
    {
        m_destroyBuilding = false;
        ConstructionMenuManager.Instance.InConstructionMode.Invoke(false);
    }
}
