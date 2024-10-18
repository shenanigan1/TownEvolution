using UnityEngine;
using static Chunk;

/// <summary>
/// 0 = Water, 1 = Sand, 2 = Dirt, 3 = Rock, 4 = Selection
/// </summary>

public class TerrainShaderHandler
{
    private TerrainGenerationParams m_generationParameters;
    private ISquareColorStrategy m_colorStrategie;

    public TerrainShaderHandler(int gridSize, TerrainGenerationParams parameters, ISquareColorStrategy colorStrategie) 
    {
        m_colorStrategie = colorStrategie;
        m_generationParameters = parameters;
        for (int i = 0; i < gridSize-1; i++)
        {
            for(int j = 0; j < gridSize-1; j++)
            {
                SetTerrain(new Vector2(i, j));
            }
        }
    }
    public void SetTerrain(Vector2 position)
    {
        float noise = Mathf.PerlinNoise((float)position.x * m_generationParameters.frequence + m_generationParameters.seed.x, (float)position.y * m_generationParameters.frequence + m_generationParameters.seed.y) * m_generationParameters.amplitude;

        if(noise >= m_generationParameters.rockLevel) 
        {
            m_colorStrategie.ChangeColor((int)position.x, (int)position.y, 3);
        }
        else if(noise >= m_generationParameters.dirtLevel)
        {
            m_colorStrategie.ChangeColor((int)position.x, (int)position.y, 2);
        }
        else if(noise >= m_generationParameters.sandLevel) 
        {
            m_colorStrategie.ChangeColor((int)position.x, (int)position.y, 1);
        }
        else
        {
            m_colorStrategie.ChangeColor((int)position.x, (int)position.y, 0);
        }
    }
}
