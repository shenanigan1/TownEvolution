using UnityEngine;

[CreateAssetMenu(fileName = "WaterTank", menuName = "MyGame/Buildings/WaterTank")]
public class WaterTank : RessourceBuilding<Water>
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
