using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.VisualScripting.Member;

public class MaterialFloatController : MaterialPropertyController
{
    [Header("Material Float Controller Settings")]
    [Tooltip("The float property to controol")]
    [SerializeField] string floatProperty;
    [Tooltip("The value to set")]
    public float value;

    private void Update()
    {
        //Set the material property
        material.SetFloat(floatProperty, value);
    }
}
