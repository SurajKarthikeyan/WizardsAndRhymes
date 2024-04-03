using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Word : MonoBehaviour
{
    #region Variables
    // [SerializeField] public Toggle puzzleToggle;

    [SerializeField] private List<string> flagsToSet = new();
    
    #endregion

    #region UnityMethods

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            foreach(string flag in flagsToSet)
            {
                FlagManager.instance.SetFlag(flag, true);
            }
            
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
