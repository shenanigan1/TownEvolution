using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Handles asynchronous loading of scenes additively with controlled activation.
/// </summary>
public class LoadScene : MonoBehaviour
{
    /// <summary>
    /// Automatically load "SampleScene" at start.
    /// </summary>
    private void Start()
    {
        Load("SampleScene");
    }

    /// <summary>
    /// Starts asynchronous loading of a scene by name.
    /// </summary>
    /// <param name="sceneName">Name of the scene to load additively.</param>
    public void Load(string sceneName)
    {
        StartCoroutine(LoadSceneAsync(sceneName));
    }

    /// <summary>
    /// Coroutine that loads the scene asynchronously and activates it once ready.
    /// </summary>
    /// <param name="sceneName">Name of the scene to load.</param>
    /// <returns>IEnumerator for coroutine.</returns>
    private IEnumerator LoadSceneAsync(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

        // Prevent automatic scene activation until fully loaded.
        asyncLoad.allowSceneActivation = false;

        // While the asynchronous load is still in progress...
        while (!asyncLoad.isDone)
        {
            // Report progress (progress goes from 0 to 0.9 before activation is allowed)
            Debug.Log($"Loading progress: {asyncLoad.progress * 100:F1}%");

            // When loading is essentially complete (90%), activate the scene.
            if (asyncLoad.progress >= 0.9f)
            {
                Debug.Log("Scene loaded, activating...");
                asyncLoad.allowSceneActivation = true;
            }

            // Wait for the next frame before continuing the loop.
            yield return null;
        }
    }
}
