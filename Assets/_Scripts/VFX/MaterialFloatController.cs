using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using static Unity.VisualScripting.Member;

/// <summary>
/// Controls a float property on a material
/// </summary>
public class MaterialFloatController : MaterialPropertyController
{
    #region Variables
    [Header("Material Float Controller Settings")]
    [Tooltip("The value to set")]
    [FormerlySerializedAs("value")][SerializeField] private float _value;
    public float Value
    {
        get { return _value; }
        set { _value = value; ValueUpdated(); }
    }
    

    [Tooltip("The float property to control")]
    [SerializeField] string floatProperty;
    #endregion

    #region Custom Methods

    void ValueUpdated()
    {
        //Set the material property
        material.SetFloat(floatProperty, Value);
    }
    #endregion
}
