using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Load("SampleScene");
    }

    public void Load(string sceneName)
    {
        StartCoroutine(LoadSceneAsync(sceneName));
    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        asyncLoad.allowSceneActivation = false;
        // Optional: Prevents the scene from activating immediately
        asyncLoad.allowSceneActivation = false;

        // Progressively load the scene
        while (!asyncLoad.isDone)
        {
            Debug.Log($"Loading progress: {asyncLoad.progress * 100}%");

            // If loading is almost done, activate the scene
            if (asyncLoad.progress >= 0.9f)
            {
                Debug.Log("Scene loaded, activating...");
                asyncLoad.allowSceneActivation = true;
            }

            yield return null; // Yield to the next frame
        }
    }
}
