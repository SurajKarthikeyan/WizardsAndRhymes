using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class describing the inventory of the player; what mixtapes they have
/// </summary>
public class MixtapeInventory : MonoBehaviour
{
    #region Variables
    [Tooltip("List of inventory mixtape objects in scene")]
    [SerializeField] private List<GameObject> mixtapeInventory;

    [Tooltip("Index on which mixtape the character is on")]
    private int index = 0;

    [Tooltip("Reference to the player script")]
    [SerializeField] private PlayerController player;

    
    #endregion

    #region UnityMethods

    #endregion

    #region CustomMethods
    
    
    
    /// <summary>
    /// Changes what mixtape is the "main" mixtape
    /// </summary>
    public void OnTapeChange()
    {
        int newIndex = index + 1;
        if (newIndex >= mixtapeInventory.Count)
        {
            // player.ComboCoolDown();
        }
        index = (newIndex) % mixtapeInventory.Count;
    }

    
    
    
    #endregion

}
