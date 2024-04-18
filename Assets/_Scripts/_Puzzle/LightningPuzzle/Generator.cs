using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class representing the generator in the electorblock line up
/// </summary>
public class Generator : MonoBehaviour
{
    #region Vars

    [Tooltip("Check if the generator is controlled by levers(false if controlled by electric blocks)")]
    [SerializeField] public bool isLeverControlled;
    [Tooltip("Bool to turn the generator on")]
    [SerializeField] private bool turnOn;
    [Tooltip("Generator material to turn on")]
    [SerializeField] private Material onMat;
    [Tooltip("Generator Animator")]
    [SerializeField] private Animator genAnimator;


    public Gate gate;
    
    [Tooltip("Renderer for the generator")]
    [SerializeField] private Renderer generatorRenderer;
    [Tooltip("Sound Effect for generator sound")]
    [SerializeField] private AK.Wwise.Event generatorOnSoundEffect;

    [Tooltip("If you want to change as spline emission on gate turn on")]
    [SerializeField] private IndividualEmissionChange wireEmission;

    #endregion

    #region UnityMethods



    #endregion

    #region CustomMethods
    
    /// <summary>
    /// Opens the gate :)
    /// </summary>
    public void Hodor()
    {
        gate.SwitchGate();
        generatorRenderer.material = onMat;
        generatorOnSoundEffect.Post(this.gameObject);
        if (wireEmission != null)
        {
            StartCoroutine(wireEmission.AlterEmissionOverTime(true));
        }
        genAnimator.SetBool("Active", true);
    }

    #endregion

}
 