using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBuilding : Building
{
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
        Effect();
    }

    protected override void Effect() {}

    protected override void RemoveEffect() {}

    public override void StartConsume<T>()
    { 
        if (consume || !GridManager.Instance.IsCaseHaveAccessToRessource(m_gridPosition, neededRessources))
            return;
        building.GetComponent<SpriteRenderer>().color = Color.white;
        Debug.Log("Start Consume");
        consume = true;

    }

    public override void StopConsume<T>()
    {
        if (!consume)
            return;
        building.GetComponent<SpriteRenderer>().color = Color.yellow;
        Debug.Log("End Consume");
        consume = false;
    }

}
