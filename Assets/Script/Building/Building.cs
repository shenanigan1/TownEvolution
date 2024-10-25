using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Building : ScriptableObject
{
    public int price;
    public int energyConsumption;
    public int waterConsumption;
    public GameObject buildingPrefab;
    public GameObject building;
    protected Vector2Int m_gridPosition;
    public List<Type> neededRessources = new List<Type>();
    public bool consume = false;
    public abstract void Place(Vector2Int position);
    public abstract void Destroy();
    protected abstract void Effect();
    protected abstract void RemoveEffect();
    public abstract void StartConsume<T>() where T : IRessource;
    public abstract void StopConsume<T>() where T : IRessource;
}
