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

    #endregion
    #region Unity Methods
    /// <summary>
    /// Collision function to check if the player ran into it
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameEnd.gameEnd.OpenDoorUI();   // open door to next room
        }
    }

    #endregion
}
