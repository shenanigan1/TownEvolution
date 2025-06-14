using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Singleton that manages all game resources (gold, water, energy, etc.).
/// Provides APIs to register, add, remove, and retrieve resources.
/// </summary>
public class RessourcesManager
{
    /// <summary>
    /// Internal dictionary mapping resource types to their instances.
    /// </summary>
    private Dictionary<Type, IRessource> m_ressources = new Dictionary<Type, IRessource>();

    /// <summary>
    /// Singleton instance of the RessourcesManager.
    /// </summary>
    private static RessourcesManager _instance;
    public static RessourcesManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = new RessourcesManager();
            return _instance;
        }
    }

    /// <summary>
    /// Initializes and registers all default game resources.
    /// </summary>
    public RessourcesManager()
    {
        // Register core resources
        RegisterRessource(new Gold());
        RegisterRessource(new People());
        RegisterRessource(new Water());
        RegisterRessource(new Energy());
        RegisterRessource(new RoadAccess());
        RegisterRessource(new Impot());
        RegisterRessource(new Happiness());
    }

    /// <summary>
    /// Registers a resource instance if it hasn't been registered yet.
    /// </summary>
    /// <typeparam name="T">Type of the resource.</typeparam>
    /// <param name="ressource">The instance to register.</param>
    public void RegisterRessource<T>(T ressource) where T : IRessource
    {
        Type type = typeof(T);
        if (m_ressources.ContainsKey(type))
            return;

        m_ressources[type] = ressource;
    }

    /// <summary>
    /// Adds a given amount to a registered resource.
    /// </summary>
    /// <typeparam name="T">Type of the resource.</typeparam>
    /// <param name="amount">Amount to add.</param>
    public void AddRessources<T>(int amount) where T : IRessource
    {
        Type type = typeof(T);
        if (!IsExisting(type))
            return;

        m_ressources[type].Add(amount);
    }

    /// <summary>
    /// Removes a given amount from a registered resource.
    /// </summary>
    /// <typeparam name="T">Type of the resource.</typeparam>
    /// <param name="amount">Amount to remove.</param>
    public void RemoveRessources<T>(int amount) where T : IRessource
    {
        Type type = typeof(T);
        if (!IsExisting(type))
            return;

        m_ressources[type].Remove(amount);
    }

    /// <summary>
    /// Returns the instance of a specific registered resource.
    /// </summary>
    /// <typeparam name="T">Type of the resource.</typeparam>
    /// <returns>Instance of the resource, or default if not registered.</returns>
    public T GetRessources<T>() where T : IRessource
    {
        Type type = typeof(T);
        if (!IsExisting(type))
            return default;

        return (T)m_ressources[type];
    }

    /// <summary>
    /// Verifies if a resource type is registered. Logs an error otherwise.
    /// </summary>
    /// <param name="type">Type of the resource to check.</param>
    /// <returns>True if registered, false otherwise.</returns>
    private bool IsExisting(Type type)
    {
        if (!m_ressources.ContainsKey(type))
        {
            Debug.LogWarning($"[RessourcesManager] Tried to use unregistered resource: {type.Name}. Register it first.");
            return false;
        }
        return true;
    }
}


/// <summary>
/// Interface representing a generic game resource (gold, water, energy, etc.).
/// </summary>
public interface IRessource
{
    /// <summary>
    /// Current quantity of the resource.
    /// </summary>
    int quantity { get; }

    /// <summary>
    /// Adds an amount to the resource.
    /// </summary>
    /// <param name="value">Amount to add.</param>
    void Add(int value);

    /// <summary>
    /// Removes an amount from the resource.
    /// </summary>
    /// <param name="value">Amount to remove.</param>
    void Remove(int value);
}

