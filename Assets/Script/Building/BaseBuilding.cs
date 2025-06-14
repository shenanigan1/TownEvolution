using UnityEngine;

/// <summary>
/// Abstract base class for all functional buildings placed on the grid.
/// Handles placement, consumption logic, activation/deactivation of effects, and destruction.
/// </summary>
public class BaseBuilding : Building
{
    /// <summary>
    /// Destroys the building's GameObject and unregisters it from the grid and systems.
    /// </summary>
    public override void Destroy()
    {
        Destroy(building); // Destroy visual GameObject
        RemoveEffect();    // Remove active effect (if any)
        GridManager.Instance.RemoveBuilding(this); // Remove from grid tracking
        Destroy(this);     // Destroy this ScriptableObject instance
    }

    /// <summary>
    /// Instantiates the building at the given grid position, removes gold cost, and attempts to activate it.
    /// </summary>
    /// <param name="position">The grid position to place the building on.</param>
    public override void Place(Vector2Int position)
    {
        consume = false;

        // Instantiate the building prefab at scaled world coordinates (10 units per tile)
        building = Instantiate(buildingPrefab, new Vector3(position.x * 10, 0, position.y * 10), buildingPrefab.transform.rotation);
        m_gridPosition = position;

        // Deduct construction cost
        RessourcesManager.Instance.RemoveRessources<Gold>(price);

        // Attempt to activate consumption and effects
        StartConsume();

        // If not consuming, show a warning color
        if (!consume)
            building.GetComponent<SpriteRenderer>().color = Color.yellow;
    }

    /// <summary>
    /// Placeholder for applying building-specific effects. 
    /// To be overridden by child classes.
    /// </summary>
    protected override void Effect() { }

    /// <summary>
    /// Placeholder for removing building-specific effects.
    /// To be overridden by child classes.
    /// </summary>
    protected override void RemoveEffect() { }

    /// <summary>
    /// Starts the building's resource consumption and enables its effects if all required resources are present.
    /// </summary>
    public override void StartConsume()
    {
        if (consume || !GridManager.Instance.IsCellHaveAccessToResource(m_gridPosition, neededRessources))
            return;

        building.GetComponent<SpriteRenderer>().color = Color.white; // Building is now active
        Effect();   // Apply building-specific effects
        consume = true;
        state = EStateBuilding.Work;
    }

    /// <summary>
    /// Stops the building's resource consumption and disables its effects if required resources are missing.
    /// </summary>
    public override void StopConsume()
    {
        if (!consume || GridManager.Instance.IsCellHaveAccessToResource(m_gridPosition, neededRessources))
            return;

        building.GetComponent<SpriteRenderer>().color = Color.yellow; // Show visual that building is inactive
        RemoveEffect();   // Remove building-specific effects
        consume = false;
        state = EStateBuilding.Disable;
    }
}
