using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.VFX;

/// <summary>
/// Controls a float property on a VFX graph
/// </summary>
public class VFXGraphFloatController : MonoBehaviour
{
    #region Variables
    [Tooltip("The value to set it to")]
    [FormerlySerializedAs("value")][SerializeField] private float _value;
    public float Value {
        get { return _value; }
        set { _value = value; ValueUpdated(); }
    }

    [Tooltip("The VFX to control")]
    [SerializeField] VisualEffect vfx;
    [Tooltip("The property on the VFX to set")]
    [SerializeField] string floatProperty;
    #endregion

    #region Custom Methods
    /// <summary>
    /// Update the float value on the VFX graph
    /// </summary>
    void ValueUpdated()
    {
        if (vfx != null)
            vfx.SetFloat(floatProperty, Value);
    }
    #endregion
}