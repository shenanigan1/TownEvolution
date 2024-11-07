using System;
using System.Runtime.CompilerServices;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class Chunk : MonoBehaviour
{

    [SerializeField] private TerrainGenerationParams m_generationParameter;
    private TerrainShaderHandler m_terrainGenerator;
    public ObjectPlacement m_placement;

    private int m_gridSize;
    private float m_cellSize;

    [Serializable]
    public struct TerrainGenerationParams
    {
        [Header("Noise Parameters")]
        public Vector2 seed;
        public float amplitude;
        [Range(0,1)] public float frequence;

        [Header("Biome Parameters")]
        public float sandLevel;
        public float rockLevel;
        public float dirtLevel;
    }

    private MeshFilter meshFilter;
    private SquareColorStrategy m_colorStrategy;

    RessourcesManager manager = new RessourcesManager();

    private void Start()
    {
        m_gridSize = ChunckManager.Instance.GetGridSize;
        m_cellSize = ChunckManager.Instance.GetCellSize;

        meshFilter = GetComponent<MeshFilter>();
        InitializeMesh();
        m_colorStrategy = new SquareColorStrategy(meshFilter, m_gridSize, m_cellSize);
        Gameloop.Instance.SetColorStrategie(m_colorStrategy);
        m_terrainGenerator =  new TerrainShaderHandler(m_gridSize, m_generationParameter, m_colorStrategy);
        //m_placement.PlaceObject(m_gridSize, m_cellSize);
    }

    private void InitializeMesh()
    {
        Mesh mesh = MeshFactory.CreateMesh(m_gridSize, m_cellSize);
        meshFilter.mesh = mesh;
        GetComponent<MeshCollider>().sharedMesh = mesh;
    }

    public void ChangeTile(Vector2 position, ECaseType type)
    {
        m_colorStrategy.ChangeColor((int)position.x, (int)position.y, (int)type);
    }

    private void Update()
    {

        //////////////////// To Delete ///////////////
        //MouseInputHandler.ProcessMouseInput(m_colorStrategy, ref lastChange, m_gridSize);
        if (Input.GetKey(KeyCode.R))
        {
            m_terrainGenerator = new TerrainShaderHandler(m_gridSize, m_generationParameter, m_colorStrategy);
        }

        if (Input.GetKey(KeyCode.C))
        {
            RessourcesManager.Instance.AddRessources<Gold>(10);
            RessourcesManager.Instance.AddRessources<People>(10);
        }
        //////////////////////////////////////////////
    }
}

// Interface for the color change
[UnityEngine.Scripting.Preserve]
public interface ISquareColorStrategy
{
    void ChangeColor(int x, int y, int targetSubmesh);
    int GridSize { get; }
    float CellSize { get; }
}