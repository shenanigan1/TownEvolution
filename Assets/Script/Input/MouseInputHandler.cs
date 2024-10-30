using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// Class For the handle of mouse input
public static class MouseInputHandler
{
    public static void ProcessMouseInput(SquareColorStrategy m_colorStrategy, ref Vector2Int lastChange, int gridSize, ECaseType type)
    {
        ResetLastChange(m_colorStrategy, ref lastChange);

        if (Cursor.lockState == CursorLockMode.Locked)
        {

            return;
        }

        Vector2Int? position = GridPositionUtility.GetGridPosition(Input.mousePosition);


        if (position != null && new Vector2Int(position.Value.x, position.Value.y) != lastChange)
        {
            position = new Vector2Int(position.Value.y, position.Value.x);
            if (IsValidSquareIndex(position.Value.x, position.Value.y, m_colorStrategy))
            {
                if (lastChange.x != position.Value.x || lastChange.y != position.Value.y)
                {
                    lastChange.Set(position.Value.x, position.Value.y);
                    m_colorStrategy.ChangeColor(position.Value.x, position.Value.y,(int)type);
                }
            }
        }
    }

    private static void ResetLastChange(ISquareColorStrategy m_colorStrategy, ref Vector2Int lastChange)
    {
        if (lastChange.x < 0)
            return;

        m_colorStrategy.ChangeColor(lastChange.x, lastChange.y, GridManager.Instance.GetTileType(new Vector2Int(lastChange.x, lastChange.y)));
        lastChange.Set(-1, -1);
    }
    private static bool IsValidSquareIndex(int x, int y, ISquareColorStrategy m_colorStrategy)
    {
        return x >= 0 && x < m_colorStrategy.GridSize && y >= 0 && y < m_colorStrategy.GridSize;
    }
}