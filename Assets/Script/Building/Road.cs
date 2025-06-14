using UnityEngine;

/// <summary>
/// ScriptableObject representing a Road building.
/// Roads provide <see cref="RoadAccess"/> to nearby buildings,
/// and are typically required for infrastructure connectivity.
/// </summary>
[CreateAssetMenu(fileName = "Road", menuName = "MyGame/Buildings/Road")]
public class Road : RessourceBuilding<RoadAccess>
{

}
