using AYellowpaper.SerializedCollections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Invokes methods on other MonoBehaviours to the beat of the music
/// </summary>
public class TriggerOnBeat : MonoBehaviour
{
    #region Variables
    [Tooltip("The beat interval to trigger on (ex: B4 = every quarter note)")]
    [SerializeField] WwiseAdapter.BeatIntervals beatInterval;
    [Tooltip("The number of beats to wait after triggering before triggering again")]
    [SerializeField] int beatDelay = 0;
    [Tooltip("The number of beats to wait before triggering the first time")]
    [SerializeField] int initialBeatDelay = 0;
    [Tooltip("The events to invoke")]
    [SerializeField] UnityEvent eventsToTrigger;

    [Tooltip("Tracks how many beats to wait before triggering again")]
    int beatCounter = 0;
    #endregion

    #region Unity Methods
    private void Awake()
    {
        //Register listener
        WwiseAdapter.beatEvents[beatInterval] += BeatTriggered;

        //Initial beat delay
        beatCounter = initialBeatDelay;
    }
    #endregion

    #region Custom Methods
    /// <summary>
    /// Invoked when a beat occurs
    /// </summary>
    void BeatTriggered()
    {
        if (beatCounter <= 0)
        {
            eventsToTrigger?.Invoke(); //Invoke the event(s) set in the inspector
            beatCounter = beatDelay;
        }
        else
            beatCounter--;
    }
    #endregion
}
