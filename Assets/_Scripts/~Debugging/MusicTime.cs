using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicTime : MonoBehaviour
{

    public AK.Wwise.RTPC test;
    public List<GameObject> electricPoints;
    public int multipler;

    public void Update()
    {
        float x = test.GetGlobalValue();
        float normalized = (x + 48) / 48;
        
        Vector3 newtrans = electricPoints[0].transform.localPosition;
        newtrans.y = normalized * multipler;
        electricPoints[0].transform.localPosition = newtrans;

        newtrans = electricPoints[2].transform.localPosition;
        newtrans.y = normalized * multipler;
        electricPoints[2].transform.localPosition = newtrans;
        
        
        newtrans = electricPoints[1].transform.localPosition;
        newtrans.y = -normalized * multipler;
        electricPoints[1].transform.localPosition = newtrans;
        
        newtrans = electricPoints[3].transform.localPosition;
        newtrans.y = -normalized * multipler;
        electricPoints[3].transform.localPosition = newtrans;
    }



}
