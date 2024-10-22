using System.Collections.Generic;
using UnityEngine;

public class BuildingPlacementManager : MonoBehaviour
{
    private bool m_placeBuilding = false;
    private bool m_destroyBuilding = false;
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
            Vector2Int notNullPosition = new Vector2Int(position.Value.x, position.Value.y);
            _preview.transform.position = new Vector3(position.Value.x * 10, 0, position.Value.y * 10);

            if (!IsOccupied(notNullPosition))
                return;

            if (Input.GetKeyDown(KeyCode.W))
            {
                GridManager.Instance.GetBuilding(new Vector2Int(position.Value.x, position.Value.y)).Destroy();
            }
        }
    }

    private void PlaceBuildingOnMousePosOnMap()
    {
        Vector2Int? position = GridPositionUtility.GetGridPosition(Input.mousePosition);

        if (position != null)
        {
            Vector2Int notNullPosition = new Vector2Int(position.Value.x, position.Value.y);
            _preview.transform.position = new Vector3(position.Value.x * 10, 0, position.Value.y * 10);

            if (!CanPlaceNChangePreviewColor(notNullPosition, m_currentBuilding.price))
                 return;

            if(Input.GetKeyDown(KeyCode.W))
            {
                CreateBuilding(notNullPosition);
            }
        }
    }

    private bool CanPlaceNChangePreviewColor(Vector2Int position, int price)
    {
        if (IsOccupied(position) || !CanBuy(price))
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

    private bool IsOccupied(Vector2Int position)
    {
        return GridManager.Instance.GetBuilding(position) != null;
    }

    private bool CanBuy(int price)
    {
        return price <= RessourcesManager.Instance.GetRessources<Gold>().quantity;
    }

    private void CreateBuilding(Vector2Int position)
    {
        Building building = Instantiate(m_currentBuilding);
        GridManager.Instance.SetBuilding(building, position);
        building.Place(position);
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
