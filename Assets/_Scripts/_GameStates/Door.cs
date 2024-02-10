using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;

/// <summary>
/// Class that represents door. It opens.
/// </summary>
public class Door : MonoBehaviour
{
    #region Variables

    [SerializeField] private Canvas fadeToBlack;

    #endregion
    #region Unity Methods
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Door Open"); // what else did you expect?
        }
    }

    #endregion
}
