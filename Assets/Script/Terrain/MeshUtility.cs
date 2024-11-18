using System.Collections.Generic;
using UnityEngine;

public static class MeshUtility
{
    public static void SetVertices(Mesh mesh, int gridSize, float cellSize)
    {
        List<Vector3> vertices = new List<Vector3>();

        for (int i = 0; i < gridSize; i++)
        {
            for (int j = 0; j < gridSize; j++)
            {
                Vector3 vertex = new Vector3(i * cellSize, 0, j * cellSize);
                vertices.Add(vertex);
            }
        }
        mesh.SetVertices(vertices);
    }

    public static void SetTriangles(Mesh mesh, int gridSize)
    {
        List<int> triangles = new List<int>();

        for (int i = 0; i < gridSize - 1; i++)
        {
            for (int j = 0; j < gridSize - 1; j++)
            {
                int cornerIndex = i + j * gridSize;
                triangles.Add(cornerIndex);
                triangles.Add(cornerIndex + 1);
                triangles.Add(cornerIndex + 1 + gridSize);
                triangles.Add(cornerIndex);
                triangles.Add(cornerIndex + 1 + gridSize);
                triangles.Add(cornerIndex + gridSize);
            }
        }
        mesh.SetTriangles(triangles, 0);
    }

    public static List<List<int>> GetSubmeshTriangles(Mesh mesh)
    {
        List<List<int>> submeshTriangles = new List<List<int>>();
        int subMeshCount = mesh.subMeshCount;

        for (int i = 0; i < subMeshCount; i++)
        {
            List<int> triangles = new List<int>();
            mesh.GetTriangles(triangles, i);
            submeshTriangles.Add(triangles);
        }
        return submeshTriangles;
    }

    public static void ApplyTrianglesToSubmeshes(Mesh mesh, List<List<int>> submeshTriangles)
    {
        for (int i = 0; i < submeshTriangles.Count; i++)
        {
            mesh.SetTriangles(submeshTriangles[i], i);
        }
    }

    public static void SetUVs(Mesh mesh, int gridSize)
    {
        List<Vector2> uvs = new List<Vector2>();

        for (int i = 0; i < gridSize; i++)
        {
            for (int j = 0; j < gridSize; j++)
            {
                Vector2 uv = new Vector2((float)i / (gridSize - 1), (float)j / (gridSize - 1));
                uvs.Add(uv);
            }
        }
        mesh.SetUVs(0, uvs);
    }
}
