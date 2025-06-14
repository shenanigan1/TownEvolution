using UnityEngine;

/// <summary>
/// A base class for resources that automatically consume themselves each cycle,
/// and can sell any surplus.
/// </summary>
public class AutoConsume : BaseRessource, IProduction
{
    private int _consumption = 0;
    private int _sellPrice = 1;

    public int Consumption
    {
        get => _consumption;
        set => _consumption = Mathf.Max(0, value);
    }

    public int SellPrice
    {
        get => _sellPrice;
        set => _sellPrice = Mathf.Max(0, value);
    }

    /// <summary>
    /// Calculates how much of the resource can be sold after consumption.
    /// </summary>
    /// <returns>Total money gained from selling the surplus, or negative if in deficit.</returns>
    public virtual int UpdateProduction()
    {
        int surplus = quantity - Consumption;

        if (surplus > 0)
        {
            int income = surplus * SellPrice;

            // Optionally reduce quantity if you auto-sell surplus
            quantity -= surplus;

            OnSurplusSold(income, surplus);
            return income;
        }

        // Optional hook for deficit management
        OnDeficitReached(surplus);
        return surplus;
    }

    /// <summary>
    /// Called when surplus is sold. Override to trigger events or UI.
    /// </summary>
    protected virtual void OnSurplusSold(int income, int soldAmount)
    {
        // UIManager.Instance?.ResourceSold.Invoke(...);
    }

    /// <summary>
    /// Called when resource is in deficit.
    /// </summary>
    protected virtual void OnDeficitReached(int deficit)
    {
        // Could log, trigger warning, etc.
    }
}


/// <summary>
/// Interface for resources that produce and/or consume over time.
/// </summary>
public interface IProduction
{
    int Consumption { get; set; }
    int SellPrice { get; set; }

    /// <summary>
    /// Updates and returns the net production for this resource.
    /// </summary>
    int UpdateProduction();
}
