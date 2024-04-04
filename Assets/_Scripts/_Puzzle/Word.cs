using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Word : MonoBehaviour
{
    #region Variables
    [SerializeField] private List<string> flagsToSet = new();
    [SerializeField] private AK.Wwise.Event wordPickUpSoundEffect;
    #endregion

    #region UnityMethods

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            wordPickUpSoundEffect.Post(other.gameObject);
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
