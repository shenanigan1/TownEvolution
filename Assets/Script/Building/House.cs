using UnityEngine;

[CreateAssetMenu(fileName = "House", menuName = "MyGame/Buildings/House")]
public class House : Building
{
    public int habitant = 10;
    public bool consume = false;

    public override void Destroy()
    {
        Destroy(building);
        RemoveEffect();
        Destroy(this);
    }

    public override void Place(Vector2Int position)
    {
        building = Instantiate(buildingPrefab, new Vector3(position.x * 10, 0, position.y * 10), buildingPrefab.transform.rotation);
        m_gridPosition = position;
        building.GetComponent<SpriteRenderer>().color = Color.yellow;
        Effect();
    }

    protected override void Effect()
    {
        RessourcesManager.Instance.RemoveRessources<Gold>(price);
        RessourcesManager.Instance.AddRessources<People>(habitant);
        if (GridManager.Instance.IsCaseHaveAccessToEnergy(m_gridPosition))
            StartConsume<Energy>();

    }

    protected override void RemoveEffect()
    {
        RessourcesManager.Instance.RemoveRessources<People>(habitant);
        RessourcesManager.Instance.GetRessources<Energy>().consuption -= energyConsumption;
        StopConsume<Energy>();
    }

    public void StartConsume<T>() where T : AutoConsume
    {
        if (consume)
            return;
        building.GetComponent<SpriteRenderer>().color = Color.white;
        Debug.Log("Start Consume");
        consume = true;
        RessourcesManager.Instance.GetRessources<Energy>().consuption += energyConsumption;
    }

    public void StopConsume<T>() where T : AutoConsume
    {
        if (!consume)
            return;
        building.GetComponent<SpriteRenderer>().color = Color.yellow;  
        Debug.Log("End Consume");
        consume = false;
        RessourcesManager.Instance.GetRessources<Energy>().consuption -= energyConsumption;
    }
}
