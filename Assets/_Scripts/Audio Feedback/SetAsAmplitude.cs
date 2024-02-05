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
        //Read the amplitude from the Wwise Adapter singleton
        float amplitude = Mathf.Lerp(amplitudeRange.x, amplitudeRange.y, WwiseAdapter.S.amplitude);

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
