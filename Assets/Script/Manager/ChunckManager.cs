using UnityEngine;

public class ChunckManager : MonoBehaviour
{
    public static ChunckManager Instance { get; private set; }
    [SerializeField][Range(1, 256)] private int gridSize;
    [SerializeField][Range(0, 32)] private float cellSize;

    private void Awake()
    {
        if (Instance == null) 
        {
            Instance = this;
        }
        else
            Destroy(this);
    }

    public int GetGridSize => gridSize;
    public float GetCellSize => cellSize;
}
