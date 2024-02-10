using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Class that represents the door at the end of a level
/// </summary>
public class Door : MonoBehaviour
{
    #region Variables
    [SerializeField] private FadeToBlack fadeToBlack;
    [SerializeField] private Button fadeInButton;
    [SerializeField] private TextMeshProUGUI doorText;
    #endregion
    #region Unity Methods
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            fadeToBlack.OnFadeOut();
            fadeInButton.gameObject.SetActive(true);
            doorText.gameObject.SetActive(true);
        }
    }

    public void FadeButton()
    { 
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
        fadeInButton.gameObject.SetActive(false);
        doorText.gameObject.SetActive(false);
    }

    #endregion
}
