using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public abstract class RessourceBuilding<T> : BaseBuilding, IZoneEffect where T : IRessource
{
    [SerializeField]
    private int m_energy;
    [SerializeField]
    private int m_effectRadius;
    [SerializeField]
    private Type m_ressource = typeof(T);
    public int energy { get { return m_energy; } set { m_energy = value; } }
    public int effectRadius { get { return m_effectRadius; } set { m_effectRadius = value; } }
    public Type ressource { get { return m_ressource; } set { m_ressource = value; } }

    protected override void Effect()
    {
        if (!GridManager.Instance.IsCaseHaveAccessToRessource(m_gridPosition, neededRessources) || haveSetEffect)
            return;
        haveSetEffect = true;
        RessourcesManager.Instance.AddRessources<T>(energy);
        GridManager.Instance.SetRessourceProvider<T>(m_gridPosition, this);
    }

    protected override void RemoveEffect()
    {
        haveSetEffect = false;
        RessourcesManager.Instance.RemoveRessources<T>(energy);
        GridManager.Instance.RemoveRessourceProvider<T>(m_gridPosition, this);
    }

}
