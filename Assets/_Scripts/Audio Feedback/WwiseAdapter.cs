using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class WwiseAdapter : MonoBehaviour
{
    public enum BeatIntervals { B32, B16, B8, B4, B2, B1 };

    [HideInInspector] public static WwiseAdapter S; //Singleton instance (Suraj signed off on this, take it up with him)
    int beatCounter = 0; //Used to keep track of which interval events to call on each beat

    //Events and delegates
    [HideInInspector] public static event BeatDelegate on32Beat;
    [HideInInspector] public static event BeatDelegate on16Beat;
    [HideInInspector] public static event BeatDelegate on8Beat;
    [HideInInspector] public static event BeatDelegate on4Beat;
    [HideInInspector] public static event BeatDelegate on2Beat;
    [HideInInspector] public static event BeatDelegate on1Beat;

    [HideInInspector] public delegate void BeatDelegate();

    [HideInInspector] public static Dictionary<BeatIntervals, BeatDelegate> beatEvents = new Dictionary<BeatIntervals, BeatDelegate>()
    {
        {BeatIntervals.B32, on32Beat },
        {BeatIntervals.B16, on16Beat },
        {BeatIntervals.B8, on8Beat },
        {BeatIntervals.B4, on4Beat },
        {BeatIntervals.B2, on2Beat },
        {BeatIntervals.B1, on1Beat }
    };

    [HideInInspector] public float amplitude;

    [Tooltip("The RTPC to pull amplitude data from")]
    [SerializeField] AK.Wwise.RTPC amplitudeRTPC;

    private void Awake()
    {
        //Initialize singleton
        if (S == null)
            S = this;
        else
        {
            Debug.LogError("Duplicate WWise Adapter in scene " + gameObject.name);
            Destroy(this);
        }
    }

    private void OnDestroy()
    {
        //Free singleton
        if (S == this)
            S = null;
    }

    //Called by Wwise callback on each 32nd beat
    public void Wwise32Beat()
    {
        //Trigger the appropriate functions
        beatEvents[BeatIntervals.B32]?.Invoke();
        if (beatCounter % 2 == 0) { beatEvents[BeatIntervals.B16]?.Invoke(); }
        if (beatCounter % 4 == 0) { beatEvents[BeatIntervals.B8]?.Invoke(); }
        if (beatCounter % 8 == 0) { beatEvents[BeatIntervals.B4]?.Invoke(); }
        if (beatCounter % 16 == 0) { beatEvents[BeatIntervals.B2]?.Invoke(); }
        if (beatCounter % 32 == 0) { beatEvents[BeatIntervals.B1]?.Invoke(); }

        //Increment the beat counter
        beatCounter++;
        if (beatCounter == 32)
            beatCounter = 0;
    }

    private void Update()
    {
        amplitude = amplitudeRTPC.GetGlobalValue();
        amplitude = (amplitude + 48) / 48;
    }

#if UNITY_EDITOR
    [ContextMenu("Copy Function Name")]
    public void GetFunctionName()
    {
        //Copy the name of the beat function to the clipboard
        GUIUtility.systemCopyBuffer = nameof(Wwise32Beat);
    }
#endif
}
