using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

// Class For the handle of mouse input
public static class MouseInputHandler
{
    public static void ProcessMouseInput(SquareColorStrategy m_colorStrategy, ref Vector2Int lastChange, int gridSize, ECaseType type)
    {
        Vector2Int? position = GridPositionUtility.GetGridPosition(Input.mousePosition);
        

        if (Cursor.lockState == CursorLockMode.Locked || position == null || EventSystem.current.IsPointerOverGameObject())
        {
            ResetLastChange(m_colorStrategy, ref lastChange);
            lastChange.Set(-1, -1);
            return;
        }

        if(!HaveChangePosition(m_colorStrategy, lastChange, position))
        {
            return;
        }

        ResetLastChange(m_colorStrategy, ref lastChange);
        position = new Vector2Int(position.Value.y, position.Value.x);
        lastChange.Set(position.Value.x, position.Value.y);
        m_colorStrategy.ChangeColor(position.Value.x, position.Value.y, (int)type);
    }

    private static bool HaveChangePosition(SquareColorStrategy m_colorStrategy, Vector2Int lastChange, Vector2Int? position)
    {
        if (IsValidSquareIndex(position.Value.x, position.Value.y, m_colorStrategy))
        {
            if (lastChange.x != position.Value.y || lastChange.y != position.Value.x)
                return true;
        }
        return false;
    }

    private static void ResetLastChange(ISquareColorStrategy m_colorStrategy, ref Vector2Int lastChange)
    {
        if (lastChange.x < 0)
            return;

        m_colorStrategy.ChangeColor(lastChange.x, lastChange.y, GridManager.Instance.GetTileType(new Vector2Int(lastChange.x, lastChange.y)));
    }
    private static bool IsValidSquareIndex(int x, int y, ISquareColorStrategy m_colorStrategy)
    {
        return x >= 0 && x < m_colorStrategy.GridSize && y >= 0 && y < m_colorStrategy.GridSize;
    }
}