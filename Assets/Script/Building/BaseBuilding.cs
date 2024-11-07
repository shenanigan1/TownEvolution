using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBuilding : Building
{
    public override void Destroy()
    {
        Destroy(building);
        RemoveEffect();
        GridManager.Instance.RemoveBuilding(this);
        Destroy(this);
    }

    public override void Place(Vector2Int position)
    {
        consume = false;
        building = Instantiate(buildingPrefab, new Vector3(position.x * 10, 0, position.y * 10), buildingPrefab.transform.rotation);
        m_gridPosition = position;
        RessourcesManager.Instance.RemoveRessources<Gold>(price);
        StartConsume();
        if (!consume)
            building.GetComponent<SpriteRenderer>().color = Color.yellow;
    }

    protected override void Effect() {}

    protected override void RemoveEffect() {}

    public override void StartConsume()
    { 
        if (consume || !GridManager.Instance.IsCaseHaveAccessToRessource(m_gridPosition, neededRessources))
            return;
        building.GetComponent<SpriteRenderer>().color = Color.white;
        Effect();
        consume = true;
        state = EStateBuilding.Work;

    }

    public override void StopConsume()
    {
        if (!consume || GridManager.Instance.IsCaseHaveAccessToRessource(m_gridPosition, neededRessources))
            return;
        building.GetComponent<SpriteRenderer>().color = Color.yellow;
        RemoveEffect();
        consume = false;
        state = EStateBuilding.Disable;
    }

}
