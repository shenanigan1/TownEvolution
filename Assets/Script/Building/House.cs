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
    }

    protected override void Effect()
    {
        if (!GridManager.Instance.IsCaseHaveAccessToRessource(m_gridPosition, neededRessources))
            return;
        RessourcesManager.Instance.GetRessources<Energy>().consuption += energyConsumption;
        RessourcesManager.Instance.AddRessources<People>(habitant);
    }

    protected override void RemoveEffect()
    {
        RessourcesManager.Instance.RemoveRessources<People>(habitant);
        RessourcesManager.Instance.GetRessources<Energy>().consuption -= energyConsumption;
    }
}
