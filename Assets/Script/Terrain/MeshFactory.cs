using UnityEngine;

// Factory Method for create Mesh
public static class MeshFactory
{
    public static Mesh CreateMesh(int gridSize, float cellSize)
    {
        Mesh mesh = new Mesh { subMeshCount = 6 };
        MeshUtility.SetVertices(mesh, gridSize, cellSize);
        MeshUtility.SetTriangles(mesh, gridSize);
        MeshUtility.SetUVs(mesh, gridSize);
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        return mesh;
    }
}
