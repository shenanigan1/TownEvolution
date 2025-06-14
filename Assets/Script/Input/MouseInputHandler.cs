using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Static class responsible for handling mouse input related to grid tile color changes.
/// </summary>
public static class MouseInputHandler
{
    /// <summary>
    /// Processes mouse input to update the color of a grid tile based on the mouse position.
    /// </summary>
    /// <param name="colorStrategy">Strategy used to change tile colors.</param>
    /// <param name="lastChange">Reference to the last changed tile position.</param>
    /// <param name="gridSize">Size of the grid.</param>
    /// <param name="type">The type of tile to apply.</param>
    public static void ProcessMouseInput(SquareColorStrategy colorStrategy, ref Vector2Int lastChange, int gridSize, ECaseType type)
    {
        Vector2Int? mouseGridPos = GridPositionUtility.GetGridPosition(Input.mousePosition);

        // Early exit if cursor locked, pointer over UI, or invalid position
        if (Cursor.lockState == CursorLockMode.Locked || mouseGridPos == null || EventSystem.current.IsPointerOverGameObject())
        {
            ResetLastChange(colorStrategy, ref lastChange);
            lastChange.Set(-1, -1);
            return;
        }

        if (!HasPositionChanged(colorStrategy, lastChange, mouseGridPos.Value))
            return;

        // Reset color of previously changed tile before updating new one
        ResetLastChange(colorStrategy, ref lastChange);

        // Switch x and y because the strategy uses (row, column) indexing or vice versa
        Vector2Int newPos = new Vector2Int(mouseGridPos.Value.y, mouseGridPos.Value.x);

        lastChange.Set(newPos.x, newPos.y);
        colorStrategy.ChangeColor(newPos.x, newPos.y, (int)type);
    }

    /// <summary>
    /// Checks if the mouse position differs from the last changed tile.
    /// </summary>
    private static bool HasPositionChanged(SquareColorStrategy colorStrategy, Vector2Int lastChange, Vector2Int currentPos)
    {
        if (IsValidSquareIndex(currentPos.x, currentPos.y, colorStrategy))
        {
            // Compare with flipped coordinates based on usage pattern
            if (lastChange.x != currentPos.y || lastChange.y != currentPos.x)
                return true;
        }
        return false;
    }

    /// <summary>
    /// Resets the color of the last changed tile to its original state.
    /// </summary>
    private static void ResetLastChange(SquareColorStrategy colorStrategy, ref Vector2Int lastChange)
    {
        if (lastChange.x < 0)
            return;

        // Restore original tile color using GridManager's tile type
        int originalTileType = GridManager.Instance.GetTileType(new Vector2Int(lastChange.x, lastChange.y));
        colorStrategy.ChangeColor(lastChange.x, lastChange.y, originalTileType);
    }

    /// <summary>
    /// Checks if the given grid indices are within the valid grid range.
    /// </summary>
    private static bool IsValidSquareIndex(int x, int y, SquareColorStrategy colorStrategy)
    {
        return x >= 0 && x < colorStrategy.GridSize && y >= 0 && y < colorStrategy.GridSize;
    }
}
