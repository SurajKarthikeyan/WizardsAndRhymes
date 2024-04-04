using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour
{
    #region Vars

    [Tooltip("Check if the generator is controlled by levers(false if controlled by electric blocks)")]
    [SerializeField] public bool isLeverControlled;
    [Tooltip("Bool to turn the generator on")]
    [SerializeField] private bool turnOn;
    [Tooltip("Generator material to turn on")]
    [SerializeField] private Material onMat;
    /*[Tooltip("Gate to open when generator is on")]
    [SerializeField] private GameObject gate;*/


    public Gate gate;
    
    [Tooltip("Renderer for the generator")]
    [SerializeField] private Renderer generatorRenderer;
    [Tooltip("Sound Effect for generator sound")]
    [SerializeField] private AK.Wwise.Event generatorOnSoundEffect;

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
    }

    #endregion

}
 