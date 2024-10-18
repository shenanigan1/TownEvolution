using System.Collections.Generic;
using UnityEngine;

public class SquareColorStrategy : ISquareColorStrategy
{
    private MeshFilter meshFilter;
    private int gridSize;
    private float cellSize;

    public SquareColorStrategy(MeshFilter filter, int size, float cell)
    {
        meshFilter = filter;
        gridSize = size;
        cellSize = cell;
    }

    public int GridSize => gridSize;
    public float CellSize => cellSize;

    public void ChangeColor(int x, int y, int targetSubmesh)
    {
        MoveSquareToSubmesh(x, y, targetSubmesh);
    }

    // Set submesh and UVs
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

    public int GetSubmeshOfSquare(int x, int y, int gridSize)
    {
        Mesh mesh = meshFilter.mesh;
        int cornerIndex = x + y * gridSize;
        List<int> squareTriangles = TriangleUtility.GetSquareTriangles(cornerIndex, gridSize);

        for (int submeshIndex = 0; submeshIndex < mesh.subMeshCount; submeshIndex++)
        {
            int[] submeshTriangles = mesh.GetTriangles(submeshIndex);

            if (ContainsAllTriangles(submeshTriangles, squareTriangles))
            {
                return submeshIndex; 
            }
        }
        return -1;
    }

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
