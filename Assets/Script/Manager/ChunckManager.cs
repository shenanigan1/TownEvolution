using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Singleton managing the chunk grid size and cell size.
/// Also handles activating the main game by enabling the UI canvas and unloading the loading scene.
/// </summary>
public class ChunckManager : MonoBehaviour
{
    public static ChunckManager Instance { get; private set; }

    [SerializeField, Range(1, 256)]
    private int gridSize = 16;

    [SerializeField, Range(0f, 32f)]
    private float cellSize = 1f;

    [SerializeField]
    private GameObject canvas;

    private void Awake()
    {
        // Singleton pattern: ensure only one instance exists
        if (Instance == null)
        {
            Instance = this;
            // Optionally, persist between scenes if needed:
            // DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Activates the main game by enabling the canvas,
    /// unloading the loading scene, and initializing the game loop.
    /// </summary>
    public void ActivateGame()
    {
        if (canvas != null)
            canvas.SetActive(true);

        // Unload the loading scene asynchronously
        SceneManager.UnloadSceneAsync("LoadScene");

        // Initialize the game loop singleton (assumed to exist)
        Gameloop.Instance.Init();
    }

    /// <summary>
    /// Gets the grid size (number of cells per dimension).
    /// </summary>
    public int GetGridSize => gridSize;

    /// <summary>
    /// Gets the size of each cell in world units.
    /// </summary>
    public float GetCellSize => cellSize;
}
