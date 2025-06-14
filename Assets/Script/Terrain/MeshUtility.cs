using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Utility class to create and manipulate grid-based Mesh data.
/// </summary>
public static class MeshUtility
{
    /// <summary>
    /// Sets the vertices of a grid mesh based on the grid size and cell size.
    /// Vertices lie on the XZ plane at y = 0.
    /// </summary>
    public static void SetVertices(Mesh mesh, int gridSize, float cellSize)
    {
        var vertices = new List<Vector3>(gridSize * gridSize);

        for (int z = 0; z < gridSize; z++)
        {
            for (int x = 0; x < gridSize; x++)
            {
                vertices.Add(new Vector3(x * cellSize, 0f, z * cellSize));
            }
        }

        mesh.SetVertices(vertices);
    }

    /// <summary>
    /// Sets the triangles of the mesh as a regular grid of quads split into two triangles each.
    /// Only sets triangles for submesh 0.
    /// </summary>
    public static void SetTriangles(Mesh mesh, int gridSize)
    {
        var triangles = new List<int>((gridSize - 1) * (gridSize - 1) * 6);

        for (int z = 0; z < gridSize - 1; z++)
        {
            for (int x = 0; x < gridSize - 1; x++)
            {
                int cornerIndex = x + z * gridSize;

                // First triangle of the quad
                triangles.Add(cornerIndex);
                triangles.Add(cornerIndex + 1);
                triangles.Add(cornerIndex + 1 + gridSize);

                // Second triangle of the quad
                triangles.Add(cornerIndex);
                triangles.Add(cornerIndex + 1 + gridSize);
                triangles.Add(cornerIndex + gridSize);
            }
        }

        mesh.SetTriangles(triangles, 0);
    }

    /// <summary>
    /// Retrieves the triangle indices for each submesh as a list of lists.
    /// </summary>
    public static List<List<int>> GetSubmeshTriangles(Mesh mesh)
    {
        var submeshTriangles = new List<List<int>>(mesh.subMeshCount);

        for (int i = 0; i < mesh.subMeshCount; i++)
        {
            var triangles = new List<int>();
            mesh.GetTriangles(triangles, i);
            submeshTriangles.Add(triangles);
        }

        return submeshTriangles;
    }

    /// <summary>
    /// Applies a list of triangle index lists back to the mesh's submeshes.
    /// </summary>
    public static void ApplyTrianglesToSubmeshes(Mesh mesh, List<List<int>> submeshTriangles)
    {
        for (int i = 0; i < submeshTriangles.Count; i++)
        {
            mesh.SetTriangles(submeshTriangles[i], i);
        }
    }

    /// <summary>
    /// Sets normalized UVs for the grid vertices ranging from 0 to 1 across the grid.
    /// </summary>
    public static void SetUVs(Mesh mesh, int gridSize)
    {
        var uvs = new List<Vector2>(gridSize * gridSize);

        for (int z = 0; z < gridSize; z++)
        {
            for (int x = 0; x < gridSize; x++)
            {
                uvs.Add(new Vector2((float)x / (gridSize - 1), (float)z / (gridSize - 1)));
            }
        }

        mesh.SetUVs(0, uvs);
    }
}
