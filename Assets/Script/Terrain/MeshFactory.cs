using UnityEngine;

/// <summary>
/// Factory class responsible for creating grid-based Mesh instances.
/// </summary>
public static class MeshFactory
{
    /// <summary>
    /// Creates a new Mesh representing a grid of quads subdivided into triangles.
    /// The mesh will have 6 submeshes by default (for different terrain types or colors).
    /// </summary>
    /// <param name="gridSize">Number of vertices along one edge of the grid.</param>
    /// <param name="cellSize">Size of each grid cell in world units.</param>
    /// <returns>A fully constructed Mesh ready to be used in a MeshFilter or MeshRenderer.</returns>
    public static Mesh CreateMesh(int gridSize, float cellSize)
    {
        var mesh = new Mesh
        {
            subMeshCount = 6
        };

        MeshUtility.SetVertices(mesh, gridSize, cellSize);
        MeshUtility.SetTriangles(mesh, gridSize);
        MeshUtility.SetUVs(mesh, gridSize);

        mesh.RecalculateBounds();
        mesh.RecalculateNormals();

        return mesh;
    }
}
