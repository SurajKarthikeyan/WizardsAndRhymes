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
    /// <summary>
    /// THIS CLASS NEEDS TO BE REFACTORED INTO THE GameEnd script because it makes more sense that way but its okay
    /// </summary>
    #region Variables

    #endregion
    #region Unity Methods
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameEnd.gameEnd.OpenDoorUI();
        }
    }

    #endregion
}
