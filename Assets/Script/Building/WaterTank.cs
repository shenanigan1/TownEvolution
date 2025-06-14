using UnityEngine;

[CreateAssetMenu(fileName = "WaterTank", menuName = "MyGame/Buildings/WaterTank")]
public class WaterTank : RessourceBuilding<Water>
{
    // Should this building consume resources over time (e.g. Water)
    [SerializeField] private bool _consumesResource = false;

    // The color to apply on the visual when the building is passive (doesn't consume)
    [SerializeField] private Color _visualColorIfNoConsumption = Color.yellow;

    // Required access type to be placed (ex: must be connected to road)
    private static readonly System.Type _requiredAccessType = typeof(RoadAccess);

    /// <summary>
    /// Called when the building is placed on the grid.
    /// Applies requirements, configuration and visual feedback.
    /// </summary>
    /// <param name="position">The grid position to place the building</param>
    public override void Place(Vector2Int position)
    {
        // Ensure the required resource dependency is added once
        if (!neededRessources.Contains(_requiredAccessType))
        {
            neededRessources.Add(_requiredAccessType);
        }

        // Set the consumption flag based on SO settings
        consume = _consumesResource;

        // Call base logic (e.g. instantiate prefab, set position, etc.)
        base.Place(position);

        // If the building doesn't consume, update its visual to indicate it
        if (!_consumesResource)
        {
            TryColorizeBuilding(_visualColorIfNoConsumption);
        }
    }

    /// <summary>
    /// Tries to color the building's SpriteRenderer if one exists.
    /// </summary>
    /// <param name="color">The color to apply</param>
    private void TryColorizeBuilding(Color color)
    {
        if (building != null && building.TryGetComponent<SpriteRenderer>(out var renderer))
        {
            renderer.color = color;
        }
    }
}
