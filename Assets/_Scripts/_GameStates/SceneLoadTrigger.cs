using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Class that handles scene loads through triggers
/// </summary>
public class SceneLoadTrigger : MonoBehaviour
{
    #region 
    [Tooltip("Scene name to load from this trigger")]
    public string sceneToLoad;

    [Tooltip("Boolean saying whether a scene load has been triggered")]
    public bool sceneLoadTriggered = false;

    [Tooltip("Scrpt reference to the fade to black to handle the fade")]
    public FadeToBlack fadeToBlack;
    #endregion

    #region Unity Methods
    /// <summary>
    /// Unity method called as fast as it's able to every frame
    /// </summary>
    private void Update()
    {
        if (sceneLoadTriggered && fadeToBlack.fadeImage.color.a >= .95f)
        {
            SceneManager.LoadScene(sceneToLoad);
        }
    }

    /// <summary>
    /// Function that is called when a collider enters it
    /// </summary>
    /// <param name="other">Collider that enters this trigger</param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (sceneToLoad != null && sceneToLoad != string.Empty)
            {
                sceneLoadTriggered = true;
                fadeToBlack.OnFadeOut();
            }
        }
    }
    #endregion
}
