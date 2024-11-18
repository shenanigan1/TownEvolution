using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI m_textMainMenu;

    [SerializeField]
    Image m_imageMainMenuBtn;

    [SerializeField]
    int m_fontMaxSize = 90;
    [SerializeField]
    int m_fontMinSize = 40;

    [SerializeField]
    float m_speed = 0.1f;

    float m_learpedSize = 0;

    private void Update()
    {
        m_learpedSize = Mathf.Clamp(m_learpedSize + m_speed*Time.deltaTime, 0, 1);

        if(m_learpedSize == 0 || m_learpedSize == 1)
            m_speed = -m_speed;

        m_textMainMenu.fontSize = Mathf.Lerp(m_fontMinSize, m_fontMaxSize, m_learpedSize);
        m_imageMainMenuBtn.color = Color.HSVToRGB(Mathf.Lerp(0,1,m_learpedSize),1,1);
    }

    public void StartGame()
    {
        SceneManager.LoadSceneAsync(1);
    }

}
