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
    [Tooltip("Types of components to control")]
    enum TargetType { FloatController, ShaderController}

    [Tooltip("Possible fields to control")]
    enum ControlField { Driven, OneTimeAmplitude}

    [Tooltip("Enumerator to determine which frequency band to listen to")]
    [HideInInspector] public enum FrequencyResponse { All, Low, Mid, High };


    [Tooltip("The type of component to control")]
    [SerializeField] TargetType targetType;

    [Tooltip("The FloatController to set a value on based on audio amplitude")]
    [MMEnumCondition("targetType", (int)TargetType.FloatController, Hidden = true)]
    [SerializeField] FloatController floatController;

    [Tooltip("The ShaderController to set a value on based on audio amplitude")]
    [MMEnumCondition("targetType", (int)TargetType.ShaderController, Hidden = true)]
    [SerializeField] ShaderController shaderController;

    [Tooltip("The field on the float controller to control")]
    [SerializeField] ControlField controlField;

    [Tooltip("Which frequency range to respond to")]
    [SerializeField] private FrequencyResponse frequencyResponse;

    [Tooltip("Range that the amplitude is remapped to fall within")]
    [SerializeField] Vector2 amplitudeRange = new Vector2(0, 1);

    [Tooltip("Whether or not to smooth the amplitude")]
    [SerializeField] bool smooth;

    [Tooltip("The maximum amount the reported amplitude can change in one second")]
    [MMCondition("smooth", Hidden = true)]
    [SerializeField] float maxAmplitudeChange = 1f;
    
    [Tooltip("Whether the amplitude should only update on beats")]
    [SerializeField] bool onlyUpdateOnBeat;
    
    [MMCondition("onlyUpdateOnBeat", true)][Tooltip("The beat interval to update the amplitude on")]
    [SerializeField] WwiseAdapter.BeatIntervals beatInterval;


    [Tooltip("Whether previous amplitude has been recorded yet or not")]
    private bool initializedPreviousAmplitude = false;
    [Tooltip("The last amplitude that was read")]
    private float previousAmplitude = 0;
    [Tooltip("The time the last amplitude was read")]
    private float previousAmplitudeTime = 0;
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
        //Read the amplitude from the Wwise Adapter singleton
        float readAmplitude = 0f;
        switch (frequencyResponse)
        {
            case FrequencyResponse.All:
                readAmplitude = WwiseAdapter.instance.amplitude;
                break;
            case FrequencyResponse.High:
                readAmplitude = WwiseAdapter.instance.highAmplitude;
                break;
            case FrequencyResponse.Mid:
                readAmplitude = WwiseAdapter.instance.midAmplitude;
                break;
            case FrequencyResponse.Low:
                readAmplitude = WwiseAdapter.instance.lowAmplitude;
                break;
        }
        float amplitude = Mathf.Lerp(amplitudeRange.x, amplitudeRange.y, readAmplitude);

        //Smooth the amplitude
        if (smooth)
        {
            if (initializedPreviousAmplitude)
            {
                float timeSinceLastAmplitude = Time.time - previousAmplitudeTime;
                amplitude = Mathf.Clamp(amplitude, previousAmplitude - maxAmplitudeChange * timeSinceLastAmplitude, previousAmplitude + maxAmplitudeChange * timeSinceLastAmplitude);
            }
            previousAmplitude = amplitude;
            previousAmplitudeTime = Time.time;
            initializedPreviousAmplitude = true;
        }

        //Apply the amplitude to the target field
        switch (targetType)
        {
            case TargetType.FloatController:
                //Apply amplitude to a FloatController
                switch (controlField)
                {
                    case ControlField.Driven:
                        floatController.DrivenLevel = amplitude;
                        break;
                    case ControlField.OneTimeAmplitude:
                        floatController.OneTimeAmplitude = amplitude;
                        break;
                }
                break;
            case TargetType.ShaderController:
                //Apply amplitude to a ShaderController
                switch (controlField)
                {
                    case ControlField.Driven:
                        shaderController.DrivenLevel = amplitude;
                        break;
                    case ControlField.OneTimeAmplitude:
                        shaderController.OneTimeAmplitude = amplitude;
                        break;
                }
                break;
        }
    }
    #endregion
}
