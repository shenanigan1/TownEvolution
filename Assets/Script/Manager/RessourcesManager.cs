using System;
using System.Collections.Generic;
using UnityEngine;

public class RessourcesManager
{
    private Dictionary<Type, IRessource> m_ressources = new Dictionary<Type, IRessource>();
    public static RessourcesManager Instance;

    public RessourcesManager() 
    {
        if (Instance == null)
            Instance = this;

        this.RegisterRessource(new Gold());
        this.RegisterRessource(new People());
        this.RegisterRessource(new Water());
        this.RegisterRessource(new Energy());
        this.RegisterRessource(new RoadAccess());
        this.RegisterRessource(new Impot());
        this.RegisterRessource(new Happiness());
    }

    public void RegisterRessource<T>(T ressource) where T : IRessource
    {
        Type type = typeof(T);
        if (m_ressources.ContainsKey(type))
            return;
        m_ressources[type] = ressource;
    }

    public void AddRessources<T>(int amount) where T : IRessource
    {
        Type type = typeof(T);

        if(!IsExisting(type))
        {
            return;
        }
        m_ressources[type].Add(amount);
    }

    public void RemoveRessources<T>(int amount) where T : IRessource
    {
        Type type = typeof(T);

        if (!IsExisting(type))
        {
            return;
        }
        m_ressources[type].Remove(amount);
    }

    public T GetRessources<T>() where T: IRessource
    {
        Type type = typeof(T);

        if (!IsExisting(type))
        {
            return default;
        }
        return (T)m_ressources[type];
    }

    private bool IsExisting(Type type)
    {
        if (!m_ressources.ContainsKey(type))
        {
            Debug.Log("Unexisting Ressources Please Save it Before Use it");
            return false;
        }
        return true;
    }

}

public interface IRessource
{
    int quantity{ get; }
    void Add(int value){ }
    void Remove(int value) { }
}
