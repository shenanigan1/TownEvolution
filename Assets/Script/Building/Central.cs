using System;
using UnityEngine;

/// <summary>
/// Represents an energy production building (e.g., power plant).
/// Provides <see cref="Energy"/> within a radius if connected to a road.
/// </summary>
[CreateAssetMenu(fileName = "Central", menuName = "MyGame/Buildings/Central")]
public class Central : RessourceBuilding<Energy>
{
    /// <summary>
    /// Called when the building is placed on the grid.
    /// Adds required resources and invokes base placement logic.
    /// </summary>
    /// <param name="position">Grid position where the building is placed.</param>
    public override void Place(Vector2Int position)
    {
        neededRessources.Add(typeof(RoadAccess));  // Requires road access to function
        base.Place(position);
    }
}


/// <summary>
/// Interface for buildings that produce a resource affecting nearby grid zones.
/// Used for buildings like power plants or water tanks.
/// </summary>
public interface IZoneEffect
{
    /// <summary>
    /// Amount of resource this building provides.
    /// </summary>
    int energy { get; set; }

    /// <summary>
    /// Radius of effect around the building's position.
    /// </summary>
    int effectRadius { get; set; }

    /// <summary>
    /// Type of resource this building provides (e.g., Energy, Water).
    /// </summary>
    Type ressource { get; set; }
}
