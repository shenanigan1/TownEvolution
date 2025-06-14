using UnityEngine;

/// <summary>
/// Handles random placement of objects within a grid.
/// </summary>
public class ObjectPlacement : MonoBehaviour
{
    [Tooltip("Prefab to spawn randomly on the grid.")]
    [SerializeField] private GameObject spawnObject;

    [Tooltip("Parent transform to hold spawned objects.")]
    [SerializeField] private Transform parent;

    /// <summary>
    /// Instantiates objects randomly on a grid based on grid size and cell spacing.
    /// </summary>
    /// <param name="gridSize">Number of cells along one axis of the grid.</param>
    /// <param name="cellSize">Size of each cell (distance between grid points).</param>
    public void PlaceObjects(int gridSize, float cellSize)
    {
        if (spawnObject == null)
        {
            Debug.LogWarning("SpawnObject prefab is not assigned.");
            return;
        }

        for (int x = 0; x < gridSize - 1; x++)
        {
            for (int y = 0; y < gridSize - 1; y++)
            {
                // 50% chance to spawn object at this cell
                if (Random.Range(0, 100) <= 50)
                {
                    float randomOffsetX = Random.Range(0f, cellSize);
                    float randomOffsetZ = Random.Range(0f, cellSize);

                    Vector3 spawnPosition = new Vector3(
                        x * cellSize + randomOffsetX,
                        0f,
                        y * cellSize + randomOffsetZ
                    );

                    Instantiate(spawnObject, spawnPosition, Quaternion.identity, parent);
                }
            }
        }
    }
}
