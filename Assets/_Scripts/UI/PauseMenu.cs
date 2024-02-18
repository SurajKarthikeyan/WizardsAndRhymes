using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// Class representing the pause menu
/// </summary>
public class PauseMenu : MonoBehaviour
{
    #region Variables
    [Tooltip("Reference to the pause menu(currently unused)")]
    [SerializeField] private GameObject pauseMenu;

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

    #endregion
}
