using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


/// <summary>
/// Class representing the pause menu
/// </summary>
public class PauseMenu : MonoBehaviour
{
    #region Variables
    [Tooltip("Reference to the pause menu(currently unused)")]
    [SerializeField] private GameObject pauseMenu;


    public GameObject deathText;

    public GameObject levelCompletionText;

    public GameObject quitButton;

    public GameObject continueButton;

    public GameObject nextLevelButton;

    public FadeToBlack fadeToBlack;
    #endregion


    #region UnityMethods

    /// <summary>
    /// Called when object is enabled, sets time scale to 0 to pause game
    /// </summary>
    private void OnEnable()
    {
        Time.timeScale = 0f;
    }

    /// <summary>
    /// Called when object is disabled, resets time scale to 1 to continue game
    /// </summary>
    private void OnDisable()
    {
        Time.timeScale = 1f;
    }
    

    #endregion

    #region CustomMethods
    
    /// <summary>
    /// Calls player controller to unpause game when resume is pressed
    /// </summary>
    public void Resume()
    {
        PlayerController.instance.Pause();
    }

    /// <summary>
    /// Function to quit game when quit button is pressed
    /// </summary>
    public void Quit()
    {
        Application.Quit();
    }

    /// <summary>
    /// Function used to reload the current scene
    /// </summary>
    public void ReloadScene()
    {
        FlagManager.hasReloaded = true;
        Debug.Log("Set reloaded to true");
        Debug.Log(FlagManager.hasReloaded);
        AkSoundEngine.StopAll();
        AkSoundEngine.SetState("PauseMenu", "NotInPauseMenu");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadScene(string sceneName)
    {
        FlagManager.instance.ClearFlags();
        AkSoundEngine.StopAll();
        AkSoundEngine.SetState("PauseMenu", "NotInPauseMenu");
        SceneManager.LoadScene(sceneName);
    }

    public void PlayerDeathUI()
    {
        fadeToBlack.OnFadeOut();
        deathText.SetActive(true);
        continueButton.SetActive(true);
        quitButton.SetActive(true);
    }

    public void SceneCompleteUI()
    {
        fadeToBlack.OnFadeOut();
        quitButton.SetActive(true);
        nextLevelButton.SetActive(true);
        levelCompletionText.SetActive(true);
    }

    #endregion
}
