using System;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Handles building and tile placement as well as building destruction in the game world.
/// Manages preview visuals, input handling, and interaction with the grid and resource managers.
/// </summary>
public class BuildingPlacementManager : MonoBehaviour
{
    private bool m_placeBuilding = false;
    private bool m_destroyBuilding = false;
    private bool m_isBuilding = false;

    private Building m_currentBuilding;
    private ECaseType m_type;
    private Selection m_selection;
    private GameObject _preview;
    private SpriteRenderer _previewRenderer;
    private Chunk m_chunk;

    private bool m_rightClick;

    /// <summary>
    /// Initialize the placement manager with required references and setup input listeners.
    /// </summary>
    /// <param name="parameter">Parameters needed for building placement.</param>
    public void Init(BuildingPlacementParams parameter)
    {
        _preview = parameter.preview;
        m_chunk = parameter.chunk;
        m_selection = parameter.selection;

        _previewRenderer = _preview.GetComponent<SpriteRenderer>();

        // Subscribe to left mouse button press event to detect right click state.
        InputManager.Instance.LeftMouseBtnPressed.AddListener(IsRightClick);
    }

    private void Update()
    {
        // Hide preview if cursor is locked or pointer is over UI to avoid interfering with UI interaction
        if (Cursor.lockState == CursorLockMode.Locked || EventSystem.current.IsPointerOverGameObject())
        {
            if (_preview.activeSelf)
                _preview.SetActive(false);
            return;
        }

        if (!_preview.activeSelf)
            _preview.SetActive(true);

        if (m_placeBuilding)
        {
            PlaceBuildingOnMousePosOnMap();
        }
        else if (m_destroyBuilding)
        {
            DestroyBuilding();
        }
    }

    /// <summary>
    /// Handles building destruction at the mouse grid position when right clicking.
    /// </summary>
    private void DestroyBuilding()
    {
        Vector2Int? position = GridPositionUtility.GetGridPosition(Input.mousePosition);
        if (position.HasValue)
        {
            // Position preview object at the grid position (scaled by 10 as tile size)
            _preview.transform.position = new Vector3(position.Value.x * 10, 0, position.Value.y * 10);

            // Only proceed if the tile has a building
            if (!IsOccupied(position.Value))
                return;

            if (m_rightClick)
            {
                GridManager.Instance.GetBuilding(position.Value).Destroy();
            }
        }
    }

    /// <summary>
    /// Handles building or tile placement logic on mouse position, with resource checks and preview color feedback.
    /// </summary>
    private void PlaceBuildingOnMousePosOnMap()
    {
        Vector2Int? position = GridPositionUtility.GetGridPosition(Input.mousePosition);
        if (position.HasValue)
        {
            _preview.transform.position = new Vector3(position.Value.x * 10, 0, position.Value.y * 10);

            // Check if placement is valid (not occupied, enough resources) and update preview color accordingly
            if (!CanPlaceNChangePreviewColor(position.Value, m_currentBuilding.price))
                return;

            if (m_rightClick)
            {
                if (m_isBuilding)
                {
                    CreateBuilding(position.Value);
                    return;
                }
                PlaceTile(position.Value, m_type);
            }
        }
    }

    /// <summary>
    /// Changes tile type and updates resources accordingly.
    /// </summary>
    private void PlaceTile(Vector2Int position, ECaseType type)
    {
        // Avoid placing tile if already same type (swapping coordinates to match your coordinate system)
        if (GridManager.Instance.GetTileType(new Vector2Int(position.y, position.x)) == (int)type)
            return;

        RessourcesManager.Instance.RemoveRessources<Gold>(m_currentBuilding.price);
        m_chunk.ChangeTile(new Vector2(position.y, position.x), type);
    }

    /// <summary>
    /// Returns whether building placement is possible at given position and updates preview color & selection status.
    /// </summary>
    /// <param name="position">Grid position to check</param>
    /// <param name="price">Price of the building</param>
    /// <returns>True if placement is valid, false otherwise.</returns>
    private bool CanPlaceNChangePreviewColor(Vector2Int position, int price)
    {
        if (IsOccupied(position) || !CanBuy(price))
        {
            // Red preview color indicates invalid placement
            if (_previewRenderer.color != Color.red || m_selection.GetIsGood())
            {
                _previewRenderer.color = Color.red;
                m_selection.SetIsGood(false);
                m_chunk.ChangeTile(new Vector2Int(position.y, position.x), ECaseType.BadSelection);
            }
            return false;
        }
        else
        {
            // Green preview color indicates valid placement
            if (_previewRenderer.color != Color.green || !m_selection.GetIsGood())
            {
                _previewRenderer.color = Color.green;
                m_selection.SetIsGood(true);
                m_chunk.ChangeTile(new Vector2Int(position.y, position.x), ECaseType.Selection);
            }
            return true;
        }
    }

    /// <summary>
    /// Checks if a building already occupies the given grid position.
    /// </summary>
    private bool IsOccupied(Vector2Int position)
    {
        return GridManager.Instance.GetBuilding(position) != null;
    }

    /// <summary>
    /// Checks if the player has enough gold resources to buy a building.
    /// </summary>
    private bool CanBuy(int price)
    {
        return price <= RessourcesManager.Instance.GetRessources<Gold>().quantity;
    }

    /// <summary>
    /// Instantiates and places a building at the given grid position.
    /// </summary>
    private void CreateBuilding(Vector2Int position)
    {
        Building building = Instantiate(m_currentBuilding);
        GridManager.Instance.SetBuilding(building, position);
        building.Place(position);
    }

    /// <summary>
    /// Starts building placement mode with the specified building.
    /// </summary>
    public void StartPlacingBuilding(Building building)
    {
        StopDestructionMode();

        _preview.SetActive(true);
        m_currentBuilding = building;
        m_isBuilding = true;
        _previewRenderer.sprite = building.buildingPrefab.GetComponent<SpriteRenderer>().sprite;
        m_placeBuilding = true;

        ConstructionMenuManager.Instance.InConstructionMode.Invoke(true);
    }

    /// <summary>
    /// Starts tile placement mode with the specified tile.
    /// </summary>
    public void StartPlacingTile(TileChange tile)
    {
        StopDestructionMode();

        m_type = tile.type;
        _previewRenderer.sprite = null;
        m_isBuilding = false;
        m_placeBuilding = true;
        m_currentBuilding = tile.building;
        _preview.SetActive(false);

        ConstructionMenuManager.Instance.InConstructionMode.Invoke(true);
    }

    /// <summary>
    /// Stops all building and tile placement activity.
    /// </summary>
    public void StopBuilding()
    {
        StopDestructionMode();

        _preview.SetActive(false);
        m_placeBuilding = false;
        m_selection.SetIsGood(true);

        ConstructionMenuManager.Instance.InConstructionMode.Invoke(false);
    }

    /// <summary>
    /// Enables building destruction mode.
    /// </summary>
    public void StartDestructionMode()
    {
        StopBuilding();

        m_destroyBuilding = true;
        _previewRenderer.sprite = null;

        ConstructionMenuManager.Instance.InConstructionMode.Invoke(true);
    }

    /// <summary>
    /// Disables building destruction mode.
    /// </summary>
    public void StopDestructionMode()
    {
        m_destroyBuilding = false;

        ConstructionMenuManager.Instance.InConstructionMode.Invoke(false);
    }

    /// <summary>
    /// Listener callback to update right click state.
    /// </summary>
    private void IsRightClick(bool place)
    {
        m_rightClick = place;
    }
}

/// <summary>
/// Parameters required for initializing the BuildingPlacementManager.
/// </summary>
[Serializable]
public struct BuildingPlacementParams
{
    public GameObject preview;
    public Chunk chunk;
    public Selection selection;
}
