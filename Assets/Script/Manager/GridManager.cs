using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Rendering.Universal.ShaderGraph;
using UnityEngine;
using UnityEngine.InputSystem;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;
    private Dictionary<Vector2Int, GridCell> m_cells = new Dictionary<Vector2Int, GridCell>();
    private List<Building> m_buildings = new List<Building>();
    public GameObject test;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    public void RemoveBuilding(Building building)
    {
        m_buildings.Remove(building);
    }
    public void SetBuilding(Building building, Vector2Int position)
    {
        CheckExistance(position);
        m_cells[position].building = building;
        m_buildings.Add(building);
    }

    public Building GetBuilding(Vector2Int position) 
    {
        return m_cells.ContainsKey(position) ? m_cells[position].building : null;
    }

    private void SetRessourceOnCell<T>(Vector2Int position, IZoneEffect zone)where T : IRessource
    {
        CheckExistance(position);

        if (!m_cells[position].ressourceProvider.ContainsKey(typeof(T)))
            m_cells[position].ressourceProvider[zone.ressource] = new List<IZoneEffect>();

        m_cells[position].ressourceProvider[zone.ressource].Add(zone);

        if (m_cells[position].building && !m_cells[position].building.consume && IsCaseHaveAccessToRessource(position, m_cells[position].building.neededRessources))
        {
            m_cells[position].building.StartConsume();
        }
    }


    private void RemoveRessourceOnCell<T>(Vector2Int position, IZoneEffect zone) where T : IRessource
    {
        CheckExistance(position);

        if (!m_cells[position].ressourceProvider[zone.ressource].Contains(zone))
            return;
        m_cells[position].ressourceProvider[zone.ressource].Remove(zone);
        if (m_cells[position].ressourceProvider[zone.ressource].Count == 0 && m_cells[position].building is Building building)
        {
            building.StopConsume();
        }
    }

    public void SetRessourceProvider<T>(Vector2Int position, IZoneEffect zone)where T : IRessource
    {
        for(int i = position.x - zone.effectRadius; i <= position.x + zone.effectRadius; i++)
        {
            for (int j = position.y - zone.effectRadius; j <= position.y + zone.effectRadius; j++) 
            {
                if(j>= 0  && i >= 0 && i < ChunckManager.Instance.GetGridSize && j < ChunckManager.Instance.GetGridSize)
                {
                    Vector2Int pos = new Vector2Int(i, j);
                    SetRessourceOnCell<T>(pos, zone);
                    Instantiate(test, new Vector3(pos.x*10, 2, pos.y*10), Quaternion.identity, m_cells[position].building.building.gameObject.transform);

                }
            }
        }
    }

    public void RemoveRessourceProvider<T>(Vector2Int position, IZoneEffect zone) where T : IRessource
    {
        for (int i = position.x - zone.effectRadius; i <= position.x + zone.effectRadius; i++)
        {
            for (int j = position.y - zone.effectRadius; j <= position.y + zone.effectRadius; j++)
            {
                if (j >= 0 && i >= 0 && i < ChunckManager.Instance.GetGridSize && j < ChunckManager.Instance.GetGridSize)
                {
                    Vector2Int pos = new Vector2Int(i, j);
                    RemoveRessourceOnCell<T>(pos, zone);
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

    public bool IsCaseHaveAccessToRessource(Vector2Int position, List<Type> ressources)
    {
        for(int i = 0; i < ressources.Count; i++)
        {
            if (!m_cells[position].ressourceProvider.ContainsKey(ressources[i]) || m_cells[position].ressourceProvider[ressources[i]].Count == 0)
            { return false; }
        }
        return true;
    }

    public void SetTile(Vector2Int position, int type)
    {
        CheckExistance(position);
        m_cells[position].type = type;
    }

    public int GetTileType(Vector2Int position) 
    {
        return m_cells[position].type;
    }

    public float GetPercentageOfBatimentDisable()
    {
        float disable = 0;
        float batiment = 0; 

        foreach(Building b in m_buildings)
        {
            switch(b.state) 
            {
                case EStateBuilding.Work: batiment++; break;
                case EStateBuilding.Disable: batiment++; disable++; break;
            }
        }
        return disable / batiment;
    }
}

public class GridCell
{
    public int type;
    public Building building { get; set; }
    public Dictionary<Type, List<IZoneEffect>> ressourceProvider { get; set; }

    public GridCell()
    {
        building = null;
        ressourceProvider = new Dictionary<Type, List<IZoneEffect>>();
    } 
}

public enum EStateBuilding
{
    None, Work, Disable
}