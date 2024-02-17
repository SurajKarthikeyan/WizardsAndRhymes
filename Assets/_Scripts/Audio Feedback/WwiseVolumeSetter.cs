using System;
using System.Collections;
using System.Collections.Generic;
using AK.Wwise;
using UnityEngine;
using UnityEngine.UI;

public class WwiseVolumeSetter : MonoBehaviour
{


    #region Variables

    public Slider masterVolumeSlider;
    public float masterVolume;
    public RTPC masterVolumeRTPC;



    #endregion

    private void Start()
    {
        ChangeVolume();
    }

    public void ChangeVolume()
    {
        masterVolume = masterVolumeSlider.value;
        masterVolumeRTPC.SetGlobalValue(masterVolume * 100);
    }
}
