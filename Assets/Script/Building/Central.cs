using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Central", menuName = "MyGame/Buildings/Central")]
public class Central : Building, IGiveEnergy
{
    [SerializeField]
    private int m_energy;
    [SerializeField]
    private int m_effectRadius;
    public int energy { get { return m_energy; } set { m_energy = value; } }
    public int effectRadius { get { return m_effectRadius; } set { m_effectRadius = value; } }

    public override void Destroy()
    {
        Destroy(building);
        RemoveEffect();
        Destroy(this);
    }

    public override void Place(Vector2Int position)
    {
        building = Instantiate(buildingPrefab, new Vector3(position.x * 10, 0, position.y * 10), buildingPrefab.transform.rotation);
        m_gridPosition = position;
        Effect();
    }

    protected override void Effect()
    {
        RessourcesManager.Instance.RemoveRessources<Gold>(price);
        RessourcesManager.Instance.AddRessources<Energy>(energy);
        GridManager.Instance.SetEnergyProvider(m_gridPosition, this);
    }

    protected override void RemoveEffect()
    {
        RessourcesManager.Instance.RemoveRessources<Energy>(energy);
        GridManager.Instance.RemoveEnergyProvider(m_gridPosition, this);
    }
}

public interface IGiveEnergy
{
    int energy { get; set; }
    int effectRadius { get; set; }
}

public interface IGiveWater
{
    int energy { get; set; }
    int effectRadius { get; set; }
}