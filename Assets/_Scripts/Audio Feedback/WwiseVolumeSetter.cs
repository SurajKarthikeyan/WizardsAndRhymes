using System;
using System.Collections;
using System.Collections.Generic;
using AK.Wwise;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class to handle all volume mixer settings from Wwise
/// </summary>
public class WwiseVolumeSetter : MonoBehaviour
{
    #region Variables

    [Tooltip("Slider reference for master volume")]
    [SerializeField] private Slider masterVolumeSlider;
    [Tooltip("Float object that represents the percentage of the slider for master volume")]
    [SerializeField] private  float masterVolume;
    [Tooltip("RTPC of the master volume")]
    [SerializeField] private RTPC masterVolumeRTPC;
    #endregion


    #region UnityMethods
    /// <summary>
    /// Called on the first frame, used to set the volume initially to the value of the slider
    /// </summary>
    private void Start()
    {
        ChangeVolume();
    }

    

    #endregion

    #region CustomMethods

    
    /// <summary>
    /// Function to set Wwise RTPC to slider value
    /// </summary>
    public void ChangeVolume()
    {
        masterVolume = masterVolumeSlider.value;
        masterVolumeRTPC.SetGlobalValue(masterVolume * 100);
    }

    #endregion

}
