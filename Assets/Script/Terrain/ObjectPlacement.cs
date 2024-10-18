using UnityEngine;

public class ObjectPlacement : MonoBehaviour
{
    public GameObject _spawnObject;
    public Transform _parent;


    public void PlaceObject(int gridSize, float cellDiff)
    {
        for (int i = 0; i < gridSize - 1; i++)
        {
            for (int j = 0; j < gridSize - 1; j++)
            {
                if (Random.Range(0, 100) > 50)
                    continue;
                GameObject go = Instantiate<GameObject>(_spawnObject, new Vector3(i * cellDiff +Random.Range(0, cellDiff),0, j * cellDiff + Random.Range(0, cellDiff)), Quaternion.identity, _parent);
            }
        }
    }
}