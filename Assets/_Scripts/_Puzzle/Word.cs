using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Word : MonoBehaviour
{
    #region Variables
    // [SerializeField] public Toggle puzzleToggle;

    [SerializeField] private string wordAcquisitionkey;
    #endregion

    #region UnityMethods

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            FlagManager.instance.SetFlag(wordAcquisitionkey, true);
            // puzzleToggle.isOn = true;
            if (FlagManager.instance.HasAllWordFlags())
            {
                //set rap rock to interactable
                Debug.Log("Rap rock interactable");
            }
            Destroy(gameObject);
        }
    }

    #endregion
}
