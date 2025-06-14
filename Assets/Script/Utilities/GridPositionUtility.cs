using UnityEngine;

/// <summary>
/// Utility class for converting screen or world positions to grid positions.
/// </summary>
public static class GridPositionUtility
{
    /// <summary>
    /// Converts a screen or world position (usually from input) to a grid coordinate (Vector2Int).
    /// Returns null if no valid grid position was found.
    /// </summary>
    /// <param name="position">Screen or world position to convert.</param>
    /// <returns>Grid position as Vector2Int, or null if invalid.</returns>
    public static Vector2Int? GetGridPosition(Vector3 position)
    {
        if (Camera.main == null)
        {
            Debug.LogWarning("GridPositionUtility: Main camera not found.");
            return null;
        }

        // Cast a ray from the camera through the position (assumed to be screen point)
        Ray ray = Camera.main.ScreenPointToRay(position);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            // Convert world hit point to local space of the hit object
            Vector3 localHitPoint = hit.transform.InverseTransformPoint(hit.point);

            // Calculate grid indices based on local coordinates and cell size
            int x = Mathf.FloorToInt(localHitPoint.z / ChunckManager.Instance.GetCellSize);
            int y = Mathf.FloorToInt(localHitPoint.x / ChunckManager.Instance.GetCellSize);

            if (IsValidSquareIndex(x, y))
            {
                // Return as (column, row) or (x, y) coordinate on the grid
                return new Vector2Int(y, x);
            }
        }

        // No valid hit or out of bounds grid position
        return null;
    }

    /// <summary>
    /// Validates whether the given grid indices are within the grid bounds.
    /// </summary>
    /// <param name="x">Grid index x (row).</param>
    /// <param name="y">Grid index y (column).</param>
    /// <returns>True if inside grid, false otherwise.</returns>
    private static bool IsValidSquareIndex(int x, int y)
    {
        int gridSize = ChunckManager.Instance.GetGridSize;
        return x >= 0 && x < gridSize && y >= 0 && y < gridSize;
    }
}
