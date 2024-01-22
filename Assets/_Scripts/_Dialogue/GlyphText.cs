using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlyphText : MonoBehaviour
{
    #region Enumerators

    public enum GlyphType
    {
        Red,
        Blue,
        Yellow
    }
    #endregion


    #region Variables
    [SerializeField] public GlyphType glyphType;

    #endregion

    #region PrivateMethods

    private void OnGlyphClick()
    {
        GlyphCleaner.glyphCleaner.OnGlyphPanelOpen(this.gameObject);
    }

    #endregion
}
