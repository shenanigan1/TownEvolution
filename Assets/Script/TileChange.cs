using UnityEngine;

/// <summary>
/// Represents a change in tile type associated with a building.
/// </summary>
public class TileChange : MonoBehaviour
{
    /// <summary>
    /// The type of the tile to change to.
    /// </summary>
    public ECaseType type;

    /// <summary>
    /// The building associated with this tile change.
    /// </summary>
    public Building building;
}
