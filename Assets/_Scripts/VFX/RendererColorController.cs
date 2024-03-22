using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// Sets a color property on all materials of a renderer according to a float input
/// </summary>
public class SingleValueShaderColorController : MonoBehaviour
{
    #region Variables
    [Tooltip("The renderer to control the materials of")]
    [SerializeField] Renderer render;
    [Tooltip("The color property on the renderer to control")]
    [SerializeField] string colorProperty;
    [FormerlySerializedAs("value")][SerializeField] private float _value;
    public float Value
    {
        get { return _value; }
        set { _value = value; ValueUpdated(); }
    }
    [Tooltip("The color to display when t=0")]
    [SerializeField][ColorUsage(true, true)] Color color1;
    [Tooltip("The color to display when t=1")]
    [SerializeField][ColorUsage(true, true)] Color color2;
    #endregion

    #region Custom Methods
    /// <summary>
    /// Apply the interpolated color to all materials when the value changes
    /// </summary>
    void ValueUpdated()
    {
        Color interpolatedColor = Color.Lerp(color1, color2, Value);

        foreach (Material mat in render.materials)
            mat.SetColor(colorProperty, interpolatedColor);
    }
    #endregion
}
