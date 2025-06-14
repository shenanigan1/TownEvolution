using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Controls the main menu UI animations and handles starting the game.
/// </summary>
public class MainMenuController : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI m_textMainMenu;
    [SerializeField] private Image m_imageMainMenuBtn;

    [Header("Font Size Settings")]
    [SerializeField] private int m_fontMaxSize = 90;
    [SerializeField] private int m_fontMinSize = 40;

    [Header("Animation Settings")]
    [SerializeField, Tooltip("Speed at which the font size and button color animate.")]
    private float m_speed = 0.1f;

    private float m_learpedSize = 0f;
    private bool m_isIncreasing = true;

    private void Update()
    {
        // Animate lerped size between 0 and 1 back and forth.
        m_learpedSize += (m_isIncreasing ? 1 : -1) * m_speed * Time.deltaTime;
        m_learpedSize = Mathf.Clamp01(m_learpedSize);

        // Reverse direction once limits are reached.
        if (m_learpedSize == 0f || m_learpedSize == 1f)
            m_isIncreasing = !m_isIncreasing;

        // Lerp font size and update the text.
        m_textMainMenu.fontSize = Mathf.Lerp(m_fontMinSize, m_fontMaxSize, m_learpedSize);

        // Animate the button color cycling through hues (0 to 1 in HSV color space).
        m_imageMainMenuBtn.color = Color.HSVToRGB(m_learpedSize, 1f, 1f);
    }

    /// <summary>
    /// Starts the game by loading the scene with build index 1 asynchronously.
    /// </summary>
    public void StartGame()
    {
        SceneManager.LoadSceneAsync(1);
    }
}
