using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class VFXGraphFloatController : MonoBehaviour
{
    [Tooltip("The value to set it to")]
    [SerializeField] public float value;

    [Tooltip("The VFX to control")]
    [SerializeField] VisualEffect vfx;
    [Tooltip("The property on the VFX to set")]
    [SerializeField] string floatProperty;

    private void Update()
    {
        if (vfx != null)
            vfx.SetFloat(floatProperty, value);
    }
}