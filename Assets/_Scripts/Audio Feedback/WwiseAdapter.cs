using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;
using static UnityEngine.UIElements.UxmlAttributeDescription;

/// <summary>
/// Adapts the music for other scripts to use
/// </summary>
public class WwiseAdapter : MonoBehaviour
{
    #region Variables
    [Tooltip("The potential intervals to trigger beats on")]
    public enum BeatIntervals { B32, B16, B8, B4, B2, B1 };
    [Tooltip("Singleton instance")]
    [HideInInspector] public static WwiseAdapter S; //Suraj signed off on this, take it up with him

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


    [Header("RTPC Values")]
    [Tooltip("Amplitude of ALL frequencies")]
    [SerializeField] AK.Wwise.RTPC amplitudeRTPC;
    [Tooltip("Amplitude of low frequencies")]
    [SerializeField] private AK.Wwise.RTPC lowAmplitudeRTPC;
    [Tooltip("Amplitude of mid frequencies")]
    [SerializeField] private AK.Wwise.RTPC midAmplitudeRTPC;
    [Tooltip("Amplitude of high frequencies")]
    [SerializeField] private AK.Wwise.RTPC highAmplitudeRTPC;

    [Tooltip("All frequency amplitude")]
    [HideInInspector] public float amplitude { get; private set; }
    [Tooltip("Low frequency amplitude")]
    [HideInInspector] public float lowAmplitude { get; private set; }
    [Tooltip("Middle frequency amplitude")]
    [HideInInspector] public float midAmplitude { get; private set; }
    [Tooltip("High frequency amplitude")]
    [HideInInspector] public float highAmplitude { get; private set; }
    [Tooltip("Used to keep track of which interval events to call on each beat")]
    int beatCounter = 0;
    #endregion

    #region Unity Methods
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

    private void Update()
    {
        // These lines read in RTPC values from WWise. 
        // RTPCs are values that are(in this context) measuring the amplitude values
        // There are a few that listen to a variety of frequency bands and one that listens to all bands
        amplitude = Normalize48(amplitudeRTPC.GetGlobalValue());
        lowAmplitude = Normalize48(lowAmplitudeRTPC.GetGlobalValue());
        midAmplitude = Normalize48(midAmplitudeRTPC.GetGlobalValue());
        highAmplitude = Normalize48(highAmplitudeRTPC.GetGlobalValue());
        
        //Debug.Log(amplitude + " | " + lowAmplitude + " | " + midAmplitude + " | " + highAmplitude);
    }

    private void OnDestroy()
    {
        //Free singleton
        if (S == this)
            S = null;
    }
    #endregion

    #region Custom Methods
    /// <summary>
    /// Should be invoked by the music AkAmbient component every 32nd beat. Triggers the appropriate beat event(s)
    /// </summary>
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

    
    /// <summary>
    /// Function to normalize WWise amplitudes from -48 -> 0db to 0- > 1f
    /// Allows for easier multiplication and scaling
    /// </summary>
    /// <param name="inputAmplitude"></param>
    /// <returns></returns>
    public float Normalize48(float inputAmplitude)
    {
        return (inputAmplitude + 48) / 48;
    }

#if UNITY_EDITOR
    /// <summary>
    /// Editor tool. Copies the Wwise32Beat() function's name to the clipboard
    /// </summary>
    [ContextMenu("Copy Function Name")]
    public void GetFunctionName()
    {
        //Copy the name of the beat function to the clipboard
        GUIUtility.systemCopyBuffer = nameof(Wwise32Beat);
    }
#endif
    #endregion
}
