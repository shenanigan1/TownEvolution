using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class BuildingPlacementManager : MonoBehaviour
{
    private bool m_placeBuilding = false;
    private bool m_destroyBuilding = false;
    private bool m_isbuilding = false;
    private Building m_currentBuilding;
    private ECaseType m_type;
    private Selection m_selection;
    private GameObject _preview;
    private SpriteRenderer _previewRenderer;
    private Chunk m_chunk;

    private bool m_rightClic;

    public void Init(BuildingPlacementParams parameter) 
    {
        _preview = parameter.preview;
        m_chunk = parameter.chunk;
        m_selection = parameter.selection;

        _previewRenderer = _preview.GetComponent<SpriteRenderer>();
        InputManager.Instance.LeftMouseBtnPressed.AddListener(IsRightClic);
    }

    // Update is called once per frame
    public void Update()
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

            if (m_rightClic)
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

            if(m_rightClic)
            {
                if(m_isbuilding)
                {
                    CreateBuilding(notNullPosition);
                    return;
                }
                PlaceTile(notNullPosition, m_type);
                    
            }
        }
    }

    private void PlaceTile(Vector2Int position, ECaseType type)
    {
        if(GridManager.Instance.GetTileType(new Vector2Int(position.y, position.x)) == (int)type)
            return;

        RessourcesManager.Instance.RemoveRessources<Gold>(m_currentBuilding.price);
        m_chunk.ChangeTile(new Vector2(position.y, position.x), type);
    }

    private bool CanPlaceNChangePreviewColor(Vector2Int position, int price)
    {
        if (IsOccupied(position) || !CanBuy(price))
        {
            if (_previewRenderer.color != Color.red || m_selection.GetIsGood())
            {
                _previewRenderer.color = Color.red;
                m_selection.SetIsGood(false);
                m_chunk.ChangeTile(new Vector2Int(position.y, position.x), ECaseType.BadSelection);
            }

            return false;
        }
        else if (_previewRenderer.color != Color.green || !m_selection.GetIsGood())
        {
            _previewRenderer.color = Color.green;
            m_selection.SetIsGood(true);
            m_chunk.ChangeTile(new Vector2Int(position.y, position.x), ECaseType.Selection);
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
        m_isbuilding = true;
        _previewRenderer.sprite = building.buildingPrefab.GetComponent<SpriteRenderer>().sprite;
        m_placeBuilding = true;
        ConstructionMenuManager.Instance.InConstructionMode.Invoke(true);
    }

    public void StartPlacingTile(TileChange tile)
    {
        StopDestructionMode();
        m_type = tile.type;
        m_isbuilding = false;
        m_placeBuilding = true;
        m_currentBuilding = tile.building;
        _preview.SetActive(false);
        ConstructionMenuManager.Instance.InConstructionMode.Invoke(true);
    }

    public void StopBuilding()
    {
        StopDestructionMode();
        _preview.SetActive(false);
        m_placeBuilding = false;
        m_selection.SetIsGood(true);
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

    private void IsRightClic(bool place)
    {
        m_rightClic = place;
    }
}

[Serializable]
public struct BuildingPlacementParams
{
    public GameObject preview;
    public Chunk chunk;
    public Selection selection;
}