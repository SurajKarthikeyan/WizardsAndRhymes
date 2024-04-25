using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Loads a new scene
/// </summary>
public class LevelSwitcher : MonoBehaviour
{
    public Image fadeImage;
    
    /// <summary>
    /// Load a scene by name
    /// </summary>
    /// <param name="levelName">The name of the scene to load</param>
    public void LoadLevel(string levelName)
    {
        if (!string.IsNullOrEmpty(levelName))
        {
            AkSoundEngine.StopAll();
            SceneManager.LoadScene(levelName);
        }
        else
        {
            Debug.LogError("Level name is not set.");
        }
    }

    /// <summary>
    /// Load a level by ID
    /// </summary>
    /// <param name="levelId">The ID of the level to load</param>
    public void LoadLevel(int levelId)
    {
        AkSoundEngine.StopAll();
        SceneManager.LoadScene(levelId);
    }

    public void LoadLevelWithFade(string levelName)
    {
        if (!string.IsNullOrEmpty(levelName) && fadeImage != null)
        {
            StartCoroutine(FadeToBlack(levelName));
        }
    }

    private IEnumerator FadeToBlack(string levelName)
    {
        while (fadeImage.color.a < 1)
        {
            float alpha = fadeImage.color.a;
            alpha += 0.1f;
            fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, alpha);
            yield return new WaitForSeconds(0.07f);
        }
        SceneManager.LoadScene(levelName);
    }
}
