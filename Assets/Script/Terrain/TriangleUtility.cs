using System.Collections.Generic;

/// <summary>
/// Utility class for managing mesh triangles related to squares on a grid.
/// </summary>
public static class TriangleUtility
{
    /// <summary>
    /// Generates the triangle indices for a single square (two triangles) in a grid mesh.
    /// The square is defined by the bottom-left corner vertex index and the grid size (number of vertices per row).
    /// </summary>
    /// <param name="cornerIndex">Index of the bottom-left vertex of the square.</param>
    /// <param name="gridSize">Number of vertices per row in the grid.</param>
    /// <returns>List of 6 triangle indices representing two triangles forming the square.</returns>
    public static List<int> GetSquareTriangles(int cornerIndex, int gridSize)
    {
        return new List<int>
        {
            cornerIndex,
            cornerIndex + 1,
            cornerIndex + 1 + gridSize,

            cornerIndex,
            cornerIndex + 1 + gridSize,
            cornerIndex + gridSize
        };
    }

    /// <summary>
    /// Removes all occurrences of the specified square's triangles from all submesh triangle lists.
    /// </summary>
    /// <param name="submeshTriangles">List containing triangle index lists for each submesh.</param>
    /// <param name="squareTriangles">Triangle indices to remove (typically from GetSquareTriangles).</param>
    public static void RemoveTrianglesFromAllSubmeshes(List<List<int>> submeshTriangles, List<int> squareTriangles)
    {
        foreach (var triangles in submeshTriangles)
        {
            RemoveTriangles(triangles, squareTriangles);
        }
    }

    /// <summary>
    /// Adds the specified square's triangles to the target submesh's triangle list.
    /// </summary>
    /// <param name="submeshTriangles">List containing triangle index lists for each submesh.</param>
    /// <param name="squareTriangles">Triangle indices to add.</param>
    /// <param name="targetSubmesh">Index of the target submesh to add triangles to.</param>
    public static void AddTrianglesToSubmesh(List<List<int>> submeshTriangles, List<int> squareTriangles, int targetSubmesh)
    {
        submeshTriangles[targetSubmesh].AddRange(squareTriangles);
    }

    /// <summary>
    /// Removes triangles from a single triangle list that match the given square triangles.
    /// </summary>
    /// <param name="triangles">Triangle index list to remove from.</param>
    /// <param name="squareTriangles">Triangle indices to remove.</param>
    private static void RemoveTriangles(List<int> triangles, List<int> squareTriangles)
    {
        // Iterate backwards in steps of 3 (each triangle has 3 indices)
        for (int i = triangles.Count - 3; i >= 0; i -= 3)
        {
            if (IsMatchingTriangle(triangles, i, squareTriangles))
            {
                triangles.RemoveRange(i, 3);
            }
        }
    }

    /// <summary>
    /// Checks if the triangle at the given index matches any of the triangles defined in squareTriangles.
    /// A match is defined as all three vertex indices being contained in squareTriangles.
    /// </summary>
    /// <param name="triangles">Triangle index list.</param>
    /// <param name="index">Starting index of the triangle to check.</param>
    /// <param name="squareTriangles">Triangle indices to match against.</param>
    /// <returns>True if it matches, false otherwise.</returns>
    private static bool IsMatchingTriangle(List<int> triangles, int index, List<int> squareTriangles)
    {
        // A triangle is considered matching if all its vertices are contained in squareTriangles.
        return squareTriangles.Contains(triangles[index]) &&
               squareTriangles.Contains(triangles[index + 1]) &&
               squareTriangles.Contains(triangles[index + 2]);
    }
}
