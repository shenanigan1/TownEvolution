using TMPro;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Singleton UI Manager to update resource displays (Gold, People) in the UI.
/// Uses UnityEvents to reactively update UI text fields.
/// </summary>
public class UIManager : MonoBehaviour
{
    /// <summary>
    /// Singleton instance of UIManager.
    /// </summary>
    public static UIManager Instance;

    /// <summary>
    /// Event invoked when the gold amount changes.
    /// </summary>
    public UnityEvent<int> GoldHaveChange = new UnityEvent<int>();

    /// <summary>
    /// Event invoked when the people amount changes.
    /// </summary>
    public UnityEvent<int> PeopleHaveChange = new UnityEvent<int>();

    [SerializeField]
    [Tooltip("Reference to the TextMeshProUGUI component displaying gold.")]
    private TextMeshProUGUI goldText;

    [SerializeField]
    [Tooltip("Reference to the TextMeshProUGUI component displaying people.")]
    private TextMeshProUGUI peopleText;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// Enforces singleton pattern and prevents duplicates.
    /// </summary>
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject); // Destroy duplicate instance
            return;
        }

        // Optional: persist UIManager across scenes if needed
        // DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// Start is called before the first frame update.
    /// Registers UI update methods to the resource change events.
    /// </summary>
    private void Start()
    {
        GoldHaveChange.AddListener(SetGoldText);
        PeopleHaveChange.AddListener(SetPeopleText);
    }

    /// <summary>
    /// Updates the gold text UI element.
    /// </summary>
    /// <param name="amount">New gold amount.</param>
    private void SetGoldText(int amount)
    {
        goldText.text = $"Gold : {amount}";
    }

    /// <summary>
    /// Updates the people text UI element.
    /// </summary>
    /// <param name="amount">New people amount.</param>
    private void SetPeopleText(int amount)
    {
        peopleText.text = $"People : {amount}";
    }
}
