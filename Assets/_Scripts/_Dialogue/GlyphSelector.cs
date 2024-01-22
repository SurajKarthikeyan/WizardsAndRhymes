using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GlyphSelector : MonoBehaviour
{
    //Currently is just used to enable and disable the GlyphsOptionsPanel
    
    #region Variables
    [SerializeField] private GameObject parentUIObject;
    [SerializeField] public GlyphText.GlyphType glyphType;
    #endregion

    private void Start()
    {
        parentUIObject = this.transform.parent.parent.gameObject;
    }

    public void OnSelect()
    {
        parentUIObject.SetActive(false);
    }
}
