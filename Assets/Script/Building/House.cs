using UnityEngine;

[CreateAssetMenu(fileName = "House", menuName = "MyGame/Buildings/House")]
public class House : BaseBuilding
{
    public int habitant = 10;

    public override void Place(Vector2Int position)
    {
        neededRessources.Add(typeof(Energy));
        neededRessources.Add(typeof(RoadAccess));
        base.Place(position);
        if(!consume)
            building.GetComponent<SpriteRenderer>().color = Color.yellow;
    }

    protected override void Effect()
    {
        RessourcesManager.Instance.RemoveRessources<Gold>(price);
        RessourcesManager.Instance.AddRessources<People>(habitant);
        if (GridManager.Instance.IsCaseHaveAccessToRessource(m_gridPosition, neededRessources))
            StartConsume<Energy>();

    }

    protected override void RemoveEffect()
    {
        RessourcesManager.Instance.RemoveRessources<People>(habitant);
        RessourcesManager.Instance.GetRessources<Energy>().consuption -= energyConsumption;
        StopConsume<Energy>();
    }

    public override void StartConsume<T>()
    {
        base.StartConsume<T>();
        RessourcesManager.Instance.GetRessources<Energy>().consuption += energyConsumption;
    }

    public override void StopConsume<T>()
    {
        base.StopConsume<T>();
        RessourcesManager.Instance.GetRessources<Energy>().consuption -= energyConsumption;
    }
}
