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

    [SerializeField] private GameObject pauseMenu;

    #endregion


    #region UnityMethods

    private void OnEnable()
    {
        Time.timeScale = 0f;
    }

    private void OnDisable()
    {
        Time.timeScale = 1f;
    }
    

    #endregion

    #region CustomMethods
    public void Resume()
    {
        PlayerController.instance.Pause();
    }

    public void Quit()
    {
        Application.Quit();
    }

    #endregion
}
