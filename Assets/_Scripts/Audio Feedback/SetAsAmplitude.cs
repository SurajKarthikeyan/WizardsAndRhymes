using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;

/// <summary>
/// Sets a value on a FloatController to the amplitude of the music
/// </summary>
public class SetAsAmplitude : MonoBehaviour
{
    #region Variables
    [Tooltip("Possible fields to set on a FloatController")]
    enum FloatControllerFields { Driven, OneTimeAmplitude}

    [Tooltip("Enumerator to determine which frequency band to listen to")]
    [HideInInspector] public enum FrequencyResponse { All, Low, Mid, High };

    [Tooltip("Which frequency range to respond to")]
    [SerializeField] private FrequencyResponse frequencyResponse;
    
    [Tooltip("The FloatController to set a value on based on audio amplitude")]
    [SerializeField] FloatController floatController;
    
    [Tooltip("The field on the float controller to control")]
    [SerializeField] FloatControllerFields floatControllerField;
    
    [Tooltip("Range that the amplitude is remapped to fall within")]
    [SerializeField] Vector2 amplitudeRange = new Vector2(0, 1);
    
    [Tooltip("Whether the amplitude should only update on beats")]
    [SerializeField] bool onlyUpdateOnBeat;
    
    [MMCondition("onlyUpdateOnBeat", true)][Tooltip("The beat interval to update the amplitude on")]
    [SerializeField] WwiseAdapter.BeatIntervals beatInterval;
    #endregion

    #region Unity Methods
    private void Awake()
    {
        //Register listener
        if (onlyUpdateOnBeat)
            WwiseAdapter.beatEvents[beatInterval] += BeatTriggered;
    }

    private void Update()
    {
        if (!onlyUpdateOnBeat)
            UpdateAmplitude();
    }
    #endregion

    #region Custom Methods
    void BeatTriggered()
    {
        UpdateAmplitude();
    }

    public void UpdateAmplitude()
    {
        float readAmplitude = 0f;
        //Read the amplitude from the Wwise Adapter singleton
        switch (frequencyResponse)
        {
            case FrequencyResponse.All:
                readAmplitude = WwiseAdapter.S.amplitude;
                break;
            case FrequencyResponse.High:
                readAmplitude = WwiseAdapter.S.highAmplitude;
                break;
            case FrequencyResponse.Mid:
                readAmplitude = WwiseAdapter.S.midAmplitude;
                break;
            case FrequencyResponse.Low:
                readAmplitude = WwiseAdapter.S.lowAmplitude;
                break;
        }
        float amplitude = Mathf.Lerp(amplitudeRange.x, amplitudeRange.y, readAmplitude);

        switch (floatControllerField)
        {
            case FloatControllerFields.Driven:
                floatController.DrivenLevel = amplitude;
                break;
            case FloatControllerFields.OneTimeAmplitude:
                floatController.OneTimeAmplitude = amplitude;
                break;
        }
    }
    #endregion
}
