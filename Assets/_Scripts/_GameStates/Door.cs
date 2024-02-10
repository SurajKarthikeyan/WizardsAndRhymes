using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class that represents door. It opens.
/// </summary>
public class Door : MonoBehaviour
{
    #region Variables
    [SerializeField] private FadeToBlack fadeToBlack;
    #endregion
    #region Unity Methods
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            fadeToBlack.OnFade();
        }
    }

    #endregion
}
