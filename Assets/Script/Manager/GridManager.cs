using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Singleton managing the grid, buildings, and resource zones on each cell.
/// </summary>
public class GridManager : MonoBehaviour
{
    public static GridManager Instance;

    // Dictionary mapping grid coordinates to their cell data
    private readonly Dictionary<Vector2Int, GridCell> m_cells = new();

    // List of all buildings currently placed on the grid
    private readonly List<Building> m_buildings = new();

    [SerializeField] private GameObject debugPrefab; // For testing/debugging placement

    private void Awake()
    {
        // Singleton pattern enforcement
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    /// <summary>
    /// Removes a building from the list.
    /// </summary>
    public void RemoveBuilding(Building building)
    {
        m_buildings.Remove(building);
    }

    /// <summary>
    /// Assigns a building to a cell at the given position.
    /// </summary>
    public void SetBuilding(Building building, Vector2Int position)
    {
        EnsureCellExists(position);
        m_cells[position].building = building;
        m_buildings.Add(building);
    }

    /// <summary>
    /// Returns the building at the specified position or null if none exists.
    /// </summary>
    public Building GetBuilding(Vector2Int position)
    {
        return m_cells.TryGetValue(position, out var cell) ? cell.building : null;
    }

    /// <summary>
    /// Adds a resource provider zone effect to a cell and starts building consumption if applicable.
    /// </summary>
    private void SetResourceOnCell<T>(Vector2Int position, IZoneEffect zone) where T : IRessource
    {
        EnsureCellExists(position);

        var cell = m_cells[position];

        // Initialize the list for this resource type if missing
        if (!cell.ressourceProvider.ContainsKey(typeof(T)))
            cell.ressourceProvider[typeof(T)] = new List<IZoneEffect>();

        cell.ressourceProvider[typeof(T)].Add(zone);

        // If there's a building that needs resources and currently not consuming, start consumption
        if (cell.building != null && !cell.building.consume &&
            IsCellHaveAccessToResource(position, cell.building.neededRessources))
        {
            cell.building.StartConsume();
        }
    }

    /// <summary>
    /// Removes a resource provider zone effect from a cell and stops building consumption if necessary.
    /// </summary>
    private void RemoveResourceOnCell<T>(Vector2Int position, IZoneEffect zone) where T : IRessource
    {
        EnsureCellExists(position);

        var cell = m_cells[position];

        if (!cell.ressourceProvider.TryGetValue(typeof(T), out var zoneList) || !zoneList.Contains(zone))
            return;

        zoneList.Remove(zone);

        // If no more providers for this resource and building exists, stop consumption
        if (zoneList.Count == 0 && cell.building != null)
        {
            cell.building.StopConsume();
        }
    }

    /// <summary>
    /// Adds a resource provider zone around a position, affecting cells within effect radius.
    /// </summary>
    public void SetResourceProvider<T>(Vector2Int position, IZoneEffect zone) where T : IRessource
    {
        int gridSize = ChunckManager.Instance.GetGridSize;

        for (int x = position.x - zone.effectRadius; x <= position.x + zone.effectRadius; x++)
        {
            for (int y = position.y - zone.effectRadius; y <= position.y + zone.effectRadius; y++)
            {
                if (x >= 0 && y >= 0 && x < gridSize && y < gridSize)
                {
                    Vector2Int pos = new(x, y);
                    SetResourceOnCell<T>(pos, zone);

                    // Uncomment for debug visualization of resource zones
                    // Instantiate(debugPrefab, new Vector3(pos.x * 10, 2, pos.y * 10), Quaternion.identity, m_cells[position].building.building.gameObject.transform);
                }
            }
        }
    }

    /// <summary>
    /// Removes a resource provider zone around a position.
    /// </summary>
    public void RemoveResourceProvider<T>(Vector2Int position, IZoneEffect zone) where T : IRessource
    {
        int gridSize = ChunckManager.Instance.GetGridSize;

        for (int x = position.x - zone.effectRadius; x <= position.x + zone.effectRadius; x++)
        {
            for (int y = position.y - zone.effectRadius; y <= position.y + zone.effectRadius; y++)
            {
                if (x >= 0 && y >= 0 && x < gridSize && y < gridSize)
                {
                    Vector2Int pos = new(x, y);
                    RemoveResourceOnCell<T>(pos, zone);
                }
            }
        }
    }

    /// <summary>
    /// Checks if a cell exists at the position, creates one if missing.
    /// </summary>
    private void EnsureCellExists(Vector2Int position)
    {
        if (!m_cells.ContainsKey(position))
            m_cells[position] = new GridCell();
    }

    /// <summary>
    /// Checks if the cell has access to all required resources.
    /// </summary>
    public bool IsCellHaveAccessToResource(Vector2Int position, List<Type> requiredResources)
    {
        if (!m_cells.ContainsKey(position))
            return false;

        var cell = m_cells[position];
        foreach (var resourceType in requiredResources)
        {
            if (!cell.ressourceProvider.TryGetValue(resourceType, out var providers) || providers.Count == 0)
                return false;
        }
        return true;
    }

    /// <summary>
    /// Sets the tile type at a position, creating cell if needed.
    /// </summary>
    public void SetTile(Vector2Int position, int type)
    {
        EnsureCellExists(position);
        m_cells[position].type = type;
    }

    /// <summary>
    /// Returns the tile type at a given position.
    /// </summary>
    public int GetTileType(Vector2Int position)
    {
        return m_cells.TryGetValue(position, out var cell) ? cell.type : -1;
    }

    /// <summary>
    /// Returns the ratio of disabled buildings over total buildings.
    /// </summary>
    public float GetPercentageOfBuildingDisabled()
    {
        int totalBuildings = 0;
        int disabledCount = 0;

        foreach (var building in m_buildings)
        {
            switch (building.state)
            {
                case EStateBuilding.Work:
                    totalBuildings++;
                    break;
                case EStateBuilding.Disable:
                    totalBuildings++;
                    disabledCount++;
                    break;
            }
        }

        return totalBuildings == 0 ? 0f : (float)disabledCount / totalBuildings;
    }
}

/// <summary>
/// Represents a single cell in the grid.
/// </summary>
public class GridCell
{
    public int type;
    public Building building { get; set; }
    public Dictionary<Type, List<IZoneEffect>> ressourceProvider { get; set; }

    public GridCell()
    {
        building = null;
        ressourceProvider = new Dictionary<Type, List<IZoneEffect>>();
    }
}

/// <summary>
/// Possible states of a building.
/// </summary>
public enum EStateBuilding
{
    None,
    Work,
    Disable
}
