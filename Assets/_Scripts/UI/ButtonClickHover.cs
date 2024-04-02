using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Class than handles button sound effects
/// </summary>
public class ButtonClickHover : MonoBehaviour
{
    #region Vars
    [SerializeField] private AK.Wwise.Event clickButtonSoundEffect;
    [SerializeField] private AK.Wwise.Event hoverButtonSoundEffect;

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
    }

    #endregion
}
