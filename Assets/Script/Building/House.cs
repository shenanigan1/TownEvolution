using UnityEngine;

[CreateAssetMenu(fileName = "House", menuName = "MyGame/Buildings/House")]
public class House : Building
{
    public int habitant = 10;

    public override void Destroy()
    {
        Destroy(building);
        RemoveEffect();
        Destroy(this);
    }

    public override void Place(Vector3 position)
    {
        building = Instantiate(buildingPrefab, new Vector3(position.x, position.y, position.z), buildingPrefab.transform.rotation);
        Effect();
    }

    protected override void Effect()
    {
        RessourcesManager.Instance.RemoveRessources<Gold>(price);
        RessourcesManager.Instance.AddRessources<People>(habitant);
        RessourcesManager.Instance.GetRessources<Energy>().consuption += energyConsumption;
    }

    protected override void RemoveEffect()
    {
        RessourcesManager.Instance.RemoveRessources<People>(habitant);
        RessourcesManager.Instance.GetRessources<Energy>().consuption -= energyConsumption;
    }
}
