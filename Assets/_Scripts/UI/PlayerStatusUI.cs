using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class responsible for displaying status about the player through UI elements
/// </summary>
public class PlayerStatusUI : MonoBehaviour
{
    #region Variables
    /// <summary>
    /// First word in canvas
    /// </summary>
    public GameObject word1;
    
    /// <summary>
    /// Second word in canvas
    /// </summary>
    public GameObject word2;

    /// <summary>
    /// Third word in canvas
    /// </summary>
    public GameObject word3;
    #endregion

    #region Unity Methods
    /// <summary>
    /// Function called once per frame
    /// </summary>
    void Update()
    {
        if (!word1.activeInHierarchy)
        {
            word1.SetActive(FlagManager.instance.GetFlag("word1"));
        }
        
        if (!word2.activeInHierarchy)
        {
            word2.SetActive(FlagManager.instance.GetFlag("word2"));
        }
        
        if (!word3.activeInHierarchy)
        {
            word3.SetActive(FlagManager.instance.GetFlag("word3"));
        }
    }
    #endregion
}
