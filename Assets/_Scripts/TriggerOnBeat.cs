using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerOnBeat : MonoBehaviour
{
    [Tooltip("The beat interval to trigger on (ex: B4 = every quarter note)")]
    [SerializeField] WwiseAdapter.BeatIntervals beatInterval;
    [Tooltip("The Monobehaviour to invoke a method on")]
    [SerializeField] MonoBehaviour target;
    [Tooltip("The name of the method to invoke")]
    [SerializeField] string targetMethod;

    private void Awake()
    {
        //Register listener
        WwiseAdapter.beatEvents[beatInterval] += BeatTriggered;
    }

    //Called when a beat of the selected interval is triggered
    void BeatTriggered()
    {
        target.Invoke(targetMethod, 0); //Immediately invoke the named method on the target script
    }
}
