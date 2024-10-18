using UnityEngine;

[CreateAssetMenu(fileName = "Central", menuName = "MyGame/Buildings/Central")]
public class Central : Building
{
    public int energy = 10;

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
        RessourcesManager.Instance.AddRessources<Energy>(energy);
    }

    protected override void RemoveEffect()
    {
        RessourcesManager.Instance.RemoveRessources<Energy>(energy);
    }
}
