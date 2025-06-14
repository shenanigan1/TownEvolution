using System;
using UnityEngine;

/// <summary>
/// Generic base class for all buildings that provide a specific resource type.
/// It handles resource provision logic, area of effect, and integration with the grid and resource systems.
/// </summary>
/// <typeparam name="T">The type of resource this building provides (must implement <see cref="IRessource"/>)</typeparam>
public abstract class RessourceBuilding<T> : BaseBuilding, IZoneEffect where T : IRessource
{
    [SerializeField]
    private int m_energy; // Amount of resource provided
    [SerializeField]
    private int m_effectRadius; // Radius in which the resource effect is applied
    [SerializeField]
    private Type m_ressource = typeof(T); // Runtime type of the resource (used for dynamic access if needed)

    /// <summary>
    /// Amount of resource provided by this building.
    /// </summary>
    public int energy { get => m_energy; set => m_energy = value; }

    /// <summary>
    /// Radius of effect for the resource provision.
    /// </summary>
    public int effectRadius { get => m_effectRadius; set => m_effectRadius = value; }

    /// <summary>
    /// The resource type provided (same as <typeparamref name="T"/>).
    /// </summary>
    public Type ressource { get => m_ressource; set => m_ressource = value; }

    /// <summary>
    /// Applies the resource effect to the grid and the global resource manager.
    /// Only runs if access requirements are met and the effect hasn't been applied yet.
    /// </summary>
    protected override void Effect()
    {
        if (!GridManager.Instance.IsCellHaveAccessToResource(m_gridPosition, neededRessources) || haveSetEffect)
            return;

        haveSetEffect = true;
        RessourcesManager.Instance.AddRessources<T>(energy);
        GridManager.Instance.SetResourceProvider<T>(m_gridPosition, this);
    }

    /// <summary>
    /// Removes the resource effect from the grid and the global resource manager.
    /// </summary>
    protected override void RemoveEffect()
    {
        haveSetEffect = false;
        RessourcesManager.Instance.RemoveRessources<T>(energy);
        GridManager.Instance.RemoveResourceProvider<T>(m_gridPosition, this);
    }
}
