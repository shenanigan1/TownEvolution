using UnityEngine;
using UnityEngine.EventSystems;

public class GridPositionUtility
{
    public static Vector2Int? GetGridPosition(Vector3 position)
    {
        Ray ray = Camera.main.ScreenPointToRay(position);

        if (Physics.Raycast(ray, out RaycastHit hit))
        { 
            Vector3 localHitPoint = hit.transform.InverseTransformPoint(hit.point);
            int x = Mathf.FloorToInt(localHitPoint.z / ChunckManager.Instance.GetCellSize);
            int y = Mathf.FloorToInt(localHitPoint.x / ChunckManager.Instance.GetCellSize);

            if (IsValidSquareIndex(x, y))
            {
                return new Vector2Int(y, x);
            }
        }
                return null;
    }

    private static bool IsValidSquareIndex(int x, int y)
    {
        return x >= 0 && x < ChunckManager.Instance.GetGridSize && y >= 0 && y < ChunckManager.Instance.GetGridSize;
    }
}
