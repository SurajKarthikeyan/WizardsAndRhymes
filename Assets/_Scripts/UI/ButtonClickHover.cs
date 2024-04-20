using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// Class than handles button sound effects
/// </summary>
public class ButtonClickHover : MonoBehaviour
{
    #region Vars
    [SerializeField] private AK.Wwise.Event clickButtonSoundEffect;
    [SerializeField] private AK.Wwise.Event hoverButtonSoundEffect;

    [SerializeField] private Material originalMaterial;
    [SerializeField] private Material emissionMaterial;
    [SerializeField] private Image buttonImage;

    #endregion

    #region CustomMethods

    /// <summary>
    /// Play Click Sound Effect
    /// </summary>
    public void OnClick()
    {
        clickButtonSoundEffect.Post(this.gameObject);
    }

    /// <summary>
    /// Play hover sound effect
    /// </summary>
    public void OnHover()
    {
        hoverButtonSoundEffect.Post(this.gameObject);
        buttonImage.material = emissionMaterial;
    }

    public void OnExitHover()
    {
        buttonImage.material = originalMaterial;
    }

    #endregion
}
