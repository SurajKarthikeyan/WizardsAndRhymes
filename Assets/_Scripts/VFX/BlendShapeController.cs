using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// Controls the weight of a blend shape on a skinned mesh renderer
/// </summary>
public class BlendShapeController : MonoBehaviour
{
    [Tooltip("The skinned mesh renderer to control a blend shape on")]
    [SerializeField] SkinnedMeshRenderer target;
    [Tooltip("The index of the blend shape to control on the target")]
    [SerializeField] int blendShapeIndex;

    [Tooltip("The value to set the blend shape weight to")]
    [Range(0,100)][FormerlySerializedAs("value")][SerializeField] private float _value;
    public float Value
    {
        get { return _value; }
        set { _value = value; ValueUpdated(); }
    }

    /// <summary>
    /// Update the value of the blend shape
    /// </summary>
    private void ValueUpdated()
    {
        target.SetBlendShapeWeight(blendShapeIndex, Value);
    }
}
