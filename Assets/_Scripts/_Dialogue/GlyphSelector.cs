using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GlyphSelector : MonoBehaviour
{

    
    #region Variables
    [SerializeField] private GameObject parentUIObject;
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
