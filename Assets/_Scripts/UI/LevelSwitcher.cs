using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Loads a new scene
/// </summary>
public class LevelSwitcher : MonoBehaviour
{
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
}
