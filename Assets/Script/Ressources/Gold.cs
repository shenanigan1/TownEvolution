using UnityEngine;

/// <summary>
/// Base class for all resources, implementing basic add and remove operations.
/// </summary>
public class BaseRessource : IRessource
{
    public int quantity { get; protected set; }

    /// <summary>
    /// Add value to the resource quantity.
    /// </summary>
    public virtual void Add(int value)
    {
        quantity += value;
    }

    /// <summary>
    /// Remove value from the resource quantity, clamped to zero.
    /// </summary>
    public virtual void Remove(int value)
    {
        quantity = Mathf.Max(quantity - value, 0);
    }
}

/// <summary>
/// Gold resource with UI notification on changes.
/// </summary>
public class Gold : BaseRessource
{
    public override void Add(int value)
    {
        base.Add(value);
        UIManager.Instance.GoldHaveChange.Invoke(quantity);
    }

    public override void Remove(int value)
    {
        base.Remove(value);
        UIManager.Instance.GoldHaveChange.Invoke(quantity);
    }
}

/// <summary>
/// Happiness resource
/// </summary>
public class Happiness : BaseRessource {}

/// <summary>
/// Impot resource 
/// </summary>
public class Impot : BaseRessource
{
    public override void Add(int value)
    {
        quantity = value;
    }
}

/// <summary>
/// People resource with controlled number of people and UI update on change.
/// </summary>
public class People : BaseRessource
{
    private int nbrOfPeople;
    private int m_attraction; // TODO : attraction system for the city

    public override void Add(int value)
    {
        base.Add(value);
    }

    public override void Remove(int value)
    {
        base.Remove(value);
    }

    /// <summary>
    /// Increment the number of people, clamped by the total quantity,
    /// and notify UI manager of the change.
    /// </summary>
    public int UpdatePeopleNumber()
    {
        nbrOfPeople = Mathf.Clamp(nbrOfPeople + 1, 0, quantity);
        UIManager.Instance.PeopleHaveChange.Invoke(nbrOfPeople);
        return nbrOfPeople;
    }
}
