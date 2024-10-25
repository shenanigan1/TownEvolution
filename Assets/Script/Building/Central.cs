using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Central", menuName = "MyGame/Buildings/Central")]
public class Central :RessourceBuilding<Energy>
{
    public override void Place(Vector2Int position)
    {
        neededRessources.Add(typeof(RoadAccess));
        consume = false;
        base.Place(position);
        if (!consume)
            building.GetComponent<SpriteRenderer>().color = Color.yellow;
    }
}

public interface IZoneEffect
{
    int energy { get; set; }
    int effectRadius { get; set; }
    Type ressource { get; set; }
}