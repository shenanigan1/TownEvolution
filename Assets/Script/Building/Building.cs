using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base abstract class representing a building with common properties and lifecycle methods.
/// Designed as a ScriptableObject for data-driven building definitions.
/// </summary>
public abstract class Building : ScriptableObject
{
    [Header("Building Costs & Consumption")]
    public int price;
    public int energyConsumption;
    public int waterConsumption;

    [Header("Prefab References")]
    public GameObject buildingPrefab; // Reference prefab for instantiation
    public GameObject building;       // Runtime instance of the building in the scene

    protected Vector2Int m_gridPosition; // Grid position on the map

    /// <summary>
    /// List of resource types required by this building.
    /// </summary>
    public List<Type> neededRessources = new List<Type>();

    /// <summary>
    /// Indicates whether this building is currently consuming resources.
    /// </summary>
    public bool consume = false;

    /// <summary>
    /// Internal flag indicating whether the building's effect has been applied.
    /// </summary>
    protected bool haveSetEffect = false;

    /// <summary>
    /// Current state of the building (e.g., Placed, Destroyed, etc.)
    /// </summary>
    public EStateBuilding state { get; protected set; }

    /// <summary>
    /// Place the building on the grid at the specified position.
    /// </summary>
    /// <param name="position">Grid position to place the building.</param>
    public abstract void Place(Vector2Int position);

    /// <summary>
    /// Destroy the building and cleanup.
    /// </summary>
    public abstract void Destroy();

    /// <summary>
    /// Apply the building's effect (e.g., bonuses, resource changes).
    /// </summary>
    protected abstract void Effect();

    /// <summary>
    /// Remove the building's effect.
    /// </summary>
    protected abstract void RemoveEffect();

    /// <summary>
    /// Start consuming required resources.
    /// </summary>
    public abstract void StartConsume();

    /// <summary>
    /// Stop consuming resources.
    /// </summary>
    public abstract void StopConsume();
}
