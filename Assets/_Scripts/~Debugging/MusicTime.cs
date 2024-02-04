using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicTime : MonoBehaviour
{

    public AK.Wwise.RTPC low;
    public AK.Wwise.RTPC mid;
    public AK.Wwise.RTPC high;
    public List<GameObject> electricPoints;
    public int multipler;

    public void Update()
    {
        float lowVal = low.GetGlobalValue();
        float midVal = mid.GetGlobalValue();
        float highVal = high.GetGlobalValue();
        
        Debug.Log(lowVal + " | " + midVal + " | " + highVal);
    }



}
