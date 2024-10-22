 using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;
    private Dictionary<Vector2Int, GridCell> m_cells = new Dictionary<Vector2Int, GridCell>();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }
    public void SetBuilding(Building building, Vector2Int position)
    {
        CheckExistance(position);
        m_cells[position].building = building;
    }

    public Building GetBuilding(Vector2Int position) 
    {
        return m_cells.ContainsKey(position) ? m_cells[position].building : null;
    }

    private void SetEnergyOnCell(Vector2Int position, IGiveEnergy energy)
    {
        CheckExistance(position);

        if (m_cells[position].building is House house)
        {
            house.StartConsume<Energy>();
        }

        m_cells[position].provideEnergy.Add(energy);
    }


    private void RemoveEnergyOnCell(Vector2Int position, IGiveEnergy energy)
    {
        CheckExistance(position);
        m_cells[position].provideEnergy.Remove(energy);
        if (m_cells[position].provideEnergy.Count == 0)
        {
            if (m_cells[position].building is House house)
            {
                house.StopConsume<Energy>();
            }
        }
    }

    public void SetEnergyProvider(Vector2Int position, IGiveEnergy energy)
    {
        for(int i = position.x - energy.effectRadius; i < position.x + energy.effectRadius; i++)
        {
            for (int j = position.y - energy.effectRadius; j < position.y + energy.effectRadius; j++) 
            {
                if(j>= 0  && i >= 0 && i < ChunckManager.Instance.GetGridSize && j < ChunckManager.Instance.GetGridSize)
                {
                    Vector2Int pos = new Vector2Int(i, j);
                    SetEnergyOnCell(pos, energy);
                }
            }
        }
    }

    public void RemoveEnergyProvider(Vector2Int position, IGiveEnergy energy)
    {
        for (int i = position.x - energy.effectRadius; i < position.x + energy.effectRadius; i++)
        {
            for (int j = position.y - energy.effectRadius; j < position.y + energy.effectRadius; j++)
            {
                if (j >= 0 && i >= 0 && i < ChunckManager.Instance.GetGridSize && j < ChunckManager.Instance.GetGridSize)
                {
                    Vector2Int pos = new Vector2Int(i, j);
                    RemoveEnergyOnCell(pos, energy);
                }
            }
        }
    }

    private void CheckExistance(Vector2Int position)
    {
        if(m_cells.ContainsKey(position))
        { return; }

        m_cells[position] = new GridCell();
    }

    public bool IsCaseHaveAccessToEnergy(Vector2Int position) 
    {
        return m_cells[position].provideEnergy.Count > 0;
    }

}

public class GridCell
{
    public Building building { get; set; }
    public List<IGiveEnergy> provideEnergy { get; set; }
    public List<IGiveWater> provideWater { get; set; }

    public GridCell()
    {
        building = null;
        provideEnergy = new List<IGiveEnergy>();
        provideWater = new List<IGiveWater>();
    }

}