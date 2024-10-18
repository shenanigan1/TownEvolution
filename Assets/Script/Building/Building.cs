using UnityEngine;

public abstract class Building : ScriptableObject
{
    public int price;
    public int energyConsumption;
    public int waterConsumption;
    public GameObject buildingPrefab;
    public GameObject building;
    public abstract void Place(Vector3 position);
    public abstract void Destroy();
    protected abstract void Effect();
    protected abstract void RemoveEffect();
}
