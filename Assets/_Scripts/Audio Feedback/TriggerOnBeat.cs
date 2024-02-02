using AYellowpaper.SerializedCollections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerOnBeat : MonoBehaviour
{
    [Tooltip("The beat interval to trigger on (ex: B4 = every quarter note)")]
    [SerializeField] WwiseAdapter.BeatIntervals beatInterval;

    [Tooltip("The methods to invoke")]
    [SerializedDictionary("Target", "Target Method")]
    [SerializeField] SerializedDictionary<MonoBehaviour, string> targets = new SerializedDictionary<MonoBehaviour, string>();

    private void Awake()
    {
        //Register listener
        WwiseAdapter.beatEvents[beatInterval] += BeatTriggered;
    }

    //Called when a beat of the selected interval is triggered
    void BeatTriggered()
    {
        foreach (KeyValuePair<MonoBehaviour, string> pair in targets)
        {
            pair.Key.Invoke(pair.Value, 0); //Immediately invoke the named method on the target script
        }
    }
}
