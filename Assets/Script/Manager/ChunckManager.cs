using UnityEngine;
using UnityEngine.SceneManagement;

public class ChunckManager : MonoBehaviour
{
    public static ChunckManager Instance { get; private set; }
    [SerializeField][Range(1, 256)] private int gridSize;
    [SerializeField][Range(0, 32)] private float cellSize;
    [SerializeField] GameObject canvas;


    private void Awake()
    {
        if (Instance == null) 
        {
            Instance = this;
        }
        else
            Destroy(this);
    }

    public void ActivateGame()
    {
        canvas.SetActive(true);
        SceneManager.UnloadSceneAsync("LoadScene");
        Gameloop.Instance.Init();
    }

    public int GetGridSize => gridSize;
    public float GetCellSize => cellSize;
}
