using System;
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
    private SquareColorStrategy colorStrategy;
    private Vector3Int lastChange = new Vector3Int(-1, -1, -1);

    RessourcesManager manager = new RessourcesManager();

    private void Start()
    {
        m_gridSize = ChunckManager.Instance.GetGridSize;
        m_cellSize = ChunckManager.Instance.GetCellSize;

        meshFilter = GetComponent<MeshFilter>();
        InitializeMesh();
        colorStrategy = new SquareColorStrategy(meshFilter, m_gridSize, m_cellSize);
        m_terrainGenerator =  new TerrainShaderHandler(m_gridSize, m_generationParameter, colorStrategy);
        //m_placement.PlaceObject(m_gridSize, m_cellSize);

        ////////////// To Delete ///////////
        manager = new RessourcesManager();
        /////////////////////////////////////
    }

    private void InitializeMesh()
    {
        Mesh mesh = MeshFactory.CreateMesh(m_gridSize, m_cellSize);
        meshFilter.mesh = mesh;
        GetComponent<MeshCollider>().sharedMesh = mesh;
    }

    private void Update()
    {

        //////////////////// To Delete ///////////////
        MouseInputHandler.ProcessMouseInput(colorStrategy, ref lastChange, m_gridSize);
        if (Input.GetKey(KeyCode.R))
        {
            m_terrainGenerator = new TerrainShaderHandler(m_gridSize, m_generationParameter, colorStrategy);
        }

        if (Input.GetKey(KeyCode.C))
        {
            RessourcesManager.Instance.AddRessources<Gold>(10);
            RessourcesManager.Instance.AddRessources<People>(10);
        }
        //////////////////////////////////////////////
    }
}

/// <summary>
/// To Change
/// </summary>

// Class For the handle of mouse input
public static class MouseInputHandler
{
    public static void ProcessMouseInput(SquareColorStrategy colorStrategy, ref Vector3Int lastChange, int gridSize)
    {
        ResetLastChange(colorStrategy, ref lastChange);

        if (Cursor.lockState == CursorLockMode.Locked)
        {

            return;
        }

        Vector2Int? position = GridPositionUtility.GetGridPosition(Input.mousePosition);


        if (position != null)
        {
            position = new Vector2Int(position.Value.y, position.Value.x);
            if (IsValidSquareIndex(position.Value.x, position.Value.y, colorStrategy))
            {
                if (lastChange.x != position.Value.x || lastChange.y != position.Value.y)
                {
                    lastChange.Set(position.Value.x, position.Value.y, colorStrategy.GetSubmeshOfSquare(position.Value.x,position.Value.y, gridSize));
                    colorStrategy.ChangeColor(position.Value.x, position.Value.y,4);
                }
            }
        }
    }

    private static void ResetLastChange(ISquareColorStrategy colorStrategy, ref Vector3Int lastChange)
    {
        if (lastChange.x < 0)
            return;

        colorStrategy.ChangeColor(lastChange.x, lastChange.y, lastChange.z);
        lastChange.Set(-1, -1, -1);
    }
    private static bool IsValidSquareIndex(int x, int y, ISquareColorStrategy colorStrategy)
    {
        return x >= 0 && x < colorStrategy.GridSize && y >= 0 && y < colorStrategy.GridSize;
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