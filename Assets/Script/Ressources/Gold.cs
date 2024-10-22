using UnityEngine;

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

public class BaseRessource : IRessource
{
    public int quantity { get; protected set; }
    public virtual void Add(int value)
    {
        quantity += value;
    }
    public virtual void Remove(int value)
    {
        quantity = (int)Mathf.Clamp(quantity - value, 0, Mathf.Infinity);
    }
}

public class People : BaseRessource
{
    private int nbrOfPeople;
    private int m_attraction;
    public override void Add(int value)
    {
        base.Add(value);
    }

    public override void Remove(int value)
    {
        base.Remove(value);
    }

    public int UpdatePeopleNumber()
    {
        nbrOfPeople = Mathf.Clamp(++nbrOfPeople, 0, quantity);
        UIManager.Instance.PeopleHaveChange.Invoke(nbrOfPeople);
        return nbrOfPeople;
    }
}