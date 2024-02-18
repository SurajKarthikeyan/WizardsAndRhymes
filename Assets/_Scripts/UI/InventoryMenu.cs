using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class to handle the inventory menu opening and closing(along other changes in UI when that occurs)
/// </summary>
public class InventoryMenu : MonoBehaviour
{

    #region Variables
    [Tooltip("The mixtape that the player sees when outside of all menus(bottom left)")]
    [SerializeField] private GameObject onScreenMixtapeInventory;
    #endregion


    #region CustomMethods
    /// <summary>
    /// Called when object is enabled, sets time scale to 0 to pause game
    /// </summary>
    private void OnEnable()
    {
        Time.timeScale = 0f;
        onScreenMixtapeInventory.SetActive(false);
    }

    /// <summary>
    /// Called when object is disabled, resets time scale to 1 to continue game
    /// </summary>
    private void OnDisable()
    {
        Time.timeScale = 1f;
        onScreenMixtapeInventory.SetActive(true);

    }
    #endregion
}
