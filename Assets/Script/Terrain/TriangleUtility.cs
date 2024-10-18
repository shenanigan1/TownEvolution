using System.Collections.Generic;

public static class TriangleUtility
{
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

    public static void RemoveTrianglesFromAllSubmeshes(List<List<int>> submeshTriangles, List<int> squareTriangles)
    {
        foreach (var triangles in submeshTriangles)
        {
            RemoveTriangles(triangles, squareTriangles);
        }
    }

    public static void AddTrianglesToSubmesh(List<List<int>> submeshTriangles, List<int> squareTriangles, int targetSubmesh)
    {
        submeshTriangles[targetSubmesh].AddRange(squareTriangles);
    }

    private static void RemoveTriangles(List<int> triangles, List<int> squareTriangles)
    {
        for (int i = triangles.Count - 3; i >= 0; i -= 3)
        {
            if (IsMatchingTriangle(triangles, i, squareTriangles))
            {
                triangles.RemoveRange(i, 3);
            }
        }
    }

    private static bool IsMatchingTriangle(List<int> triangles, int index, List<int> squareTriangles)
    {
        return squareTriangles.Contains(triangles[index]) &&
               squareTriangles.Contains(triangles[index + 1]) &&
               squareTriangles.Contains(triangles[index + 2]);
    }
}
