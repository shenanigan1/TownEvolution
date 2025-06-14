using System;
using System.Collections;
using UnityEngine;
using static Chunk; // Make sure this static import is actually needed.

/// <summary>
/// Handles procedural terrain generation by assigning terrain types
/// based on Perlin noise and updating tile colors accordingly.
/// 
/// Terrain Types:
/// 0 = Water, 1 = Sand, 2 = Dirt, 3 = Rock, 4 = Selection, 5 = BadSelection
/// </summary>
public class TerrainShaderHandler
{
    private readonly TerrainGenerationParams m_generationParameters;
    private readonly ISquareColorStrategy m_colorStrategy;

    /// <summary>
    /// Initializes the terrain handler and starts terrain generation coroutine.
    /// </summary>
    /// <param name="gridSize">Size of the terrain grid (assumed square).</param>
    /// <param name="parameters">Terrain generation parameters including frequency, amplitude, levels, and seed.</param>
    /// <param name="colorStrategy">Strategy interface to handle coloring of squares.</param>
    public TerrainShaderHandler(int gridSize, TerrainGenerationParams? parameters, ISquareColorStrategy colorStrategy)
    {
        m_generationParameters = parameters ?? throw new ArgumentNullException(nameof(parameters));
        m_colorStrategy = colorStrategy ?? throw new ArgumentNullException(nameof(colorStrategy));

        // Start terrain generation coroutine on ChunckManager singleton.
        ChunckManager.Instance.StartCoroutine(GenerateTerrain(gridSize));
    }

    /// <summary>
    /// Coroutine to generate terrain grid, yielding periodically to avoid frame stalls.
    /// </summary>
    /// <param name="gridSize">Size of the terrain grid.</param>
    /// <returns>IEnumerator for coroutine.</returns>
    public IEnumerator GenerateTerrain(int gridSize)
    {
        for (int i = 0; i < gridSize - 1; i++)
        {
            for (int j = 0; j < gridSize - 1; j++)
            {
                SetTerrain(new Vector2(i, j));

                // Yield every 10 steps to prevent freezing.
                if (j % 10 == 0)
                    yield return null; // Prefer null over 0 for better Unity practice.
            }
        }

        ChunckManager.Instance.ActivateGame();
    }

    /// <summary>
    /// Determines terrain type based on Perlin noise at the given position,
    /// then updates the corresponding tile color.
    /// </summary>
    /// <param name="position">Grid position.</param>
    public void SetTerrain(Vector2 position)
    {
        // Calculate Perlin noise value scaled by frequency and amplitude.
        float noise = Mathf.PerlinNoise(
            position.x * m_generationParameters.frequence + m_generationParameters.seed.x,
            position.y * m_generationParameters.frequence + m_generationParameters.seed.y
        ) * m_generationParameters.amplitude;

        // Assign terrain type based on noise thresholds.
        if (noise >= m_generationParameters.rockLevel)
        {
            SetTile(position, ECaseType.Rock);
        }
        else if (noise >= m_generationParameters.dirtLevel)
        {
            SetTile(position, ECaseType.Dirt);
        }
        else if (noise >= m_generationParameters.sandLevel)
        {
            SetTile(position, ECaseType.Sand);
        }
        else
        {
            SetTile(position, ECaseType.Water);
        }
    }

    /// <summary>
    /// Applies the color strategy to update the tile color for the given position and terrain type.
    /// </summary>
    /// <param name="position">Grid position.</param>
    /// <param name="type">Terrain case type.</param>
    public void SetTile(Vector2 position, ECaseType type)
    {
        m_colorStrategy.ChangeColor((int)position.x, (int)position.y, (int)type);
    }
}

/// <summary>
/// Terrain case types representing different terrain features.
/// </summary>
[Serializable]
public enum ECaseType
{
    Water,      // 0
    Sand,       // 1
    Dirt,       // 2
    Rock,       // 3
    Selection,  // 4
    BadSelection // 5
}
