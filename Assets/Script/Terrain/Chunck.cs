using System;
using UnityEngine;

/// <summary>
/// Represents a chunk of terrain with mesh generation, coloring, and object placement.
/// Requires a MeshFilter component on the GameObject.
/// </summary>
[RequireComponent(typeof(MeshFilter), typeof(MeshCollider))]
public class Chunk : MonoBehaviour
{
    [SerializeField]
    private TerrainGenerationParams m_generationParameter;

    private TerrainShaderHandler m_terrainGenerator;
    public ObjectPlacement m_placement;

    private int m_gridSize;
    private float m_cellSize;

    private MeshFilter meshFilter;
    private SquareColorStrategy m_colorStrategy;

    // Example RessourcesManager usage — consider injecting or singleton pattern
    // RessourcesManager manager = new RessourcesManager(); // Unused and should be removed or handled properly

    /// <summary>
    /// Parameters controlling terrain generation.
    /// </summary>
    [Serializable]
    public struct TerrainGenerationParams
    {
        [Header("Noise Parameters")]
        public Vector2 seed;
        public float amplitude;
        [Range(0f, 1f)]
        public float frequence;

        [Header("Biome Parameters")]
        public float sandLevel;
        public float rockLevel;
        public float dirtLevel;
    }

    private void Start()
    {
        m_gridSize = ChunckManager.Instance.GetGridSize;
        m_cellSize = ChunckManager.Instance.GetCellSize;

        // Randomize seed for procedural variation
        m_generationParameter.seed = new Vector2(UnityEngine.Random.Range(0, 1000), UnityEngine.Random.Range(0, 1000));

        meshFilter = GetComponent<MeshFilter>();

        InitializeMesh();

        m_colorStrategy = new SquareColorStrategy(meshFilter, m_gridSize, m_cellSize);

        // Notify the game loop about the current color strategy
        Gameloop.Instance.SetColorStrategie(m_colorStrategy);

        // Initialize the terrain generator with parameters and color strategy
        m_terrainGenerator = new TerrainShaderHandler(m_gridSize, m_generationParameter, m_colorStrategy);

        // Optionally place objects on this chunk
        // m_placement.PlaceObject(m_gridSize, m_cellSize);
    }

    /// <summary>
    /// Creates and assigns the mesh and collider based on grid size and cell size.
    /// </summary>
    private void InitializeMesh()
    {
        Mesh mesh = MeshFactory.CreateMesh(m_gridSize, m_cellSize);
        meshFilter.mesh = mesh;

        // Also update the MeshCollider with the same mesh for collision detection
        var meshCollider = GetComponent<MeshCollider>();
        if (meshCollider != null)
        {
            meshCollider.sharedMesh = mesh;
        }
    }

    /// <summary>
    /// Changes the tile color/type at the given grid position.
    /// </summary>
    /// <param name="position">Grid position as Vector2.</param>
    /// <param name="type">Tile type enum.</param>
    public void ChangeTile(Vector2 position, ECaseType type)
    {
        m_colorStrategy.ChangeColor((int)position.x, (int)position.y, (int)type);
    }

    private void Update()
    {
        // Debug & dev shortcuts — remove or refactor for production builds
        if (Input.GetKey(KeyCode.R))
        {
            // Re-generate terrain on pressing R key
            m_terrainGenerator = new TerrainShaderHandler(m_gridSize, m_generationParameter, m_colorStrategy);
        }

        if (Input.GetKey(KeyCode.C))
        {
            // Add resources for testing purposes
            RessourcesManager.Instance.AddRessources<Gold>(10);
            RessourcesManager.Instance.AddRessources<People>(10);
        }
    }
}

/// <summary>
/// Interface defining color strategy for chunk tiles.
/// </summary>
[UnityEngine.Scripting.Preserve]
public interface ISquareColorStrategy
{
    /// <summary>
    /// Change color of a tile at grid coordinates.
    /// </summary>
    /// <param name="x">X coordinate in grid</param>
    /// <param name="y">Y coordinate in grid</param>
    /// <param name="targetSubmesh">Target submesh/color index</param>
    void ChangeColor(int x, int y, int targetSubmesh);

    /// <summary>
    /// Size of the grid (number of tiles per side).
    /// </summary>
    int GridSize { get; }

    /// <summary>
    /// Size of each cell in world units.
    /// </summary>
    float CellSize { get; }
}
