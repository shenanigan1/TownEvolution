using UnityEngine;

/// <summary>
/// Represents a residential building that adds population (People) when placed.
/// Requires access to <see cref="Energy"/> and <see cref="RoadAccess"/> resources to function.
/// </summary>
[CreateAssetMenu(fileName = "House", menuName = "MyGame/Buildings/House")]
public class House : BaseBuilding
{
    /// <summary>
    /// Number of people added to the population when this house is active.
    /// </summary>
    public int habitant = 10;

    /// <summary>
    /// Called when the house is placed on the grid. Adds required resources for validation.
    /// </summary>
    /// <param name="position">Grid position where the house is placed.</param>
    public override void Place(Vector2Int position)
    {
        neededRessources.Add(typeof(Energy));       // Requires power to be active
        neededRessources.Add(typeof(RoadAccess));   // Must be connected to road
        base.Place(position);
    }

    /// <summary>
    /// Applies the effects of the house if all conditions are met (energy + road access).
    /// Adds people and increases energy consumption.
    /// </summary>
    protected override void Effect()
    {
        if (!GridManager.Instance.IsCellHaveAccessToResource(m_gridPosition, neededRessources))
            return;

        // Increase energy consumption
        RessourcesManager.Instance.GetRessources<Energy>().Consumption += energyConsumption;

        // Add inhabitants
        RessourcesManager.Instance.AddRessources<People>(habitant);
    }

    /// <summary>
    /// Removes the effects of the house when destroyed or deactivated.
    /// </summary>
    protected override void RemoveEffect()
    {
        // Remove inhabitants
        RessourcesManager.Instance.RemoveRessources<People>(habitant);

        // Decrease energy consumption
        RessourcesManager.Instance.GetRessources<Energy>().Consumption -= energyConsumption;
    }
}
