using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Implements a strategy to manage terrain square coloring by manipulating
/// mesh submeshes and UVs based on terrain types.
/// </summary>
public class SquareColorStrategy : ISquareColorStrategy
{
    private readonly MeshFilter meshFilter;
    private readonly int gridSize;
    private readonly float cellSize;

    /// <summary>
    /// Initializes the strategy with the mesh filter, grid size, and cell size.
    /// </summary>
    /// <param name="filter">MeshFilter containing the terrain mesh.</param>
    /// <param name="size">Number of squares along one grid axis.</param>
    /// <param name="cell">Size of each cell in world units.</param>
    public SquareColorStrategy(MeshFilter filter, int size, float cell)
    {
        meshFilter = filter ?? throw new System.ArgumentNullException(nameof(filter));
        gridSize = size;
        cellSize = cell;
    }

    /// <summary>Grid dimension (number of squares per axis).</summary>
    public int GridSize => gridSize;

    /// <summary>Size of each grid cell in world units.</summary>
    public float CellSize => cellSize;

    /// <summary>
    /// Changes the terrain square color by moving its triangles to the corresponding submesh,
    /// then updating the logical tile in the GridManager if applicable.
    /// </summary>
    /// <param name="x">Grid X coordinate of the square.</param>
    /// <param name="y">Grid Y coordinate of the square.</param>
    /// <param name="targetSubmesh">Target submesh index corresponding to terrain type.</param>
    public void ChangeColor(int x, int y, int targetSubmesh)
    {
        MoveSquareToSubmesh(x, y, targetSubmesh);

        // Skip logical grid update for selection types.
        var type = (ECaseType)targetSubmesh;
        if (type == ECaseType.Selection || type == ECaseType.BadSelection)
            return;

        GridManager.Instance.SetTile(new Vector2Int(x, y), targetSubmesh);
    }

    /// <summary>
    /// Moves the square's triangles from all submeshes and adds them to the target submesh.
    /// </summary>
    private void MoveSquareToSubmesh(int x, int y, int targetSubmesh)
    {
        Mesh mesh = meshFilter.mesh;
        List<List<int>> submeshTriangles = MeshUtility.GetSubmeshTriangles(mesh);

        int cornerIndex = x + y * gridSize;
        List<int> squareTriangles = TriangleUtility.GetSquareTriangles(cornerIndex, gridSize);

        TriangleUtility.RemoveTrianglesFromAllSubmeshes(submeshTriangles, squareTriangles);
        TriangleUtility.AddTrianglesToSubmesh(submeshTriangles, squareTriangles, targetSubmesh);

        MeshUtility.ApplyTrianglesToSubmeshes(mesh, submeshTriangles);
    }

    /// <summary>
    /// Returns the submesh index that currently contains the triangles of the square at (x, y).
    /// </summary>
    /// <returns>Submesh index or -1 if not found.</returns>
    public int GetSubmeshOfSquare(int x, int y, int gridSize)
    {
        Mesh mesh = meshFilter.mesh;
        int cornerIndex = x + y * gridSize;
        List<int> squareTriangles = TriangleUtility.GetSquareTriangles(cornerIndex, gridSize);

        for (int submeshIndex = 0; submeshIndex < mesh.subMeshCount; submeshIndex++)
        {
            int[] submeshTriangles = mesh.GetTriangles(submeshIndex);
            if (ContainsAllTriangles(submeshTriangles, squareTriangles))
                return submeshIndex;
        }

        return -1;
    }

    /// <summary>
    /// Checks if the submesh triangles array contains all the triangles specified in squareTriangles.
    /// </summary>
    private static bool ContainsAllTriangles(int[] submeshTriangles, List<int> squareTriangles)
    {
        for (int i = 0; i < squareTriangles.Count; i += 3)
        {
            bool found = false;
            for (int j = 0; j < submeshTriangles.Length; j += 3)
            {
                if (submeshTriangles[j] == squareTriangles[i] &&
                    submeshTriangles[j + 1] == squareTriangles[i + 1] &&
                    submeshTriangles[j + 2] == squareTriangles[i + 2])
                {
                    found = true;
                    break;
                }
            }
            if (!found)
                return false;
        }
        return true;
    }
}
