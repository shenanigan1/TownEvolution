using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Central", menuName = "MyGame/Buildings/Central")]
public class Central :RessourceBuilding<Energy>
{
    public override void Place(Vector2Int position)
    {
        neededRessources.Add(typeof(RoadAccess));
        base.Place(position);
    }
}

public interface IZoneEffect
{
    int energy { get; set; }
    int effectRadius { get; set; }
    Type ressource { get; set; }
}