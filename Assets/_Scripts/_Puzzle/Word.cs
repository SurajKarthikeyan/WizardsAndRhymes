using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Word : MonoBehaviour
{
    #region Variables
    [SerializeField] public Toggle puzzleToggle;
    #endregion

    #region UnityMethods

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameEnd.gameEnd.hasPassed = true;
            GameEnd.gameEnd.RoomCleared();
            puzzleToggle.isOn = true;
        }
    }

    #endregion
}
