using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MonKey.Extensions;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class to handle the inventory menu opening and closing(along other changes in UI when that occurs)
/// </summary>
public class InventoryMenu : MonoBehaviour
{

    #region Variables
    [Tooltip("The mixtape that the player sees when outside of all menus(bottom left)")]
    [SerializeField] private GameObject onScreenMixtapeInventory;
    [Tooltip("List of toggles representing each element")]
    [SerializeField] private Toggle[] elementalToggles;
    [Tooltip("Int represeting how many toggles, up to 2, are selected")]
    [SerializeField] private int toggleCount;
    [Tooltip("Button to swap")]
    [SerializeField] private Button swapButton;
    [Tooltip("Ref to mixtape inventory")]
    [SerializeField] private MixtapeInventory mixtapeInventory;
    #endregion


    #region UnityMethods
    /// <summary>
    /// Called when object is enabled, sets time scale to 0 to pause game
    /// </summary>
    private void OnEnable()
    {
        Time.timeScale = 0f;
        onScreenMixtapeInventory.SetActive(false);
        DisableAllChecks();
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

    #region CustomMethods

    /// <summary>
    /// Creates the list order to send to mixtape inventory
    /// </summary>
    public void CreateListOrder()
    {
        List<String> sendList = new List<String>();
        for (int i = 0; i < elementalToggles.Length; i++)
        {
            sendList.Add(elementalToggles[i].name);
        }
    }

    /// <summary>
    /// Called when a toggle is enabled or disabled
    /// If equal to 2, enable swap button and make 3rd element uninteractable
    /// Else, disable swap button and make other 2/3 elements clickable
    /// </summary>
    /// <param name="curToggle"></param>
    public void OnToggleSelect(Toggle curToggle)
    {
        if (toggleCount < 2 && curToggle.isOn)  // if we have less than 2 selected && it was turned on
        {
            toggleCount += 1;
            if (toggleCount == 2)
            {
                swapButton.interactable = true;
                DisableLastElement();
            }
        }

        else if (!curToggle.isOn)    // else if it was turned off
        {
            swapButton.interactable = false;
            toggleCount -= 1;
            EnableRemainingElements();
        }
    }

    /// <summary>
    /// Disables the element that isn't toggled
    /// </summary>
    private void DisableLastElement()
    {
        for(int i = 0; i<elementalToggles.Length; i++)
        {
            if (!elementalToggles[i].isOn)
            {
                elementalToggles[i].interactable = false;
            }
        }
    }

    /// <summary>
    /// Enables all the elements
    /// </summary>
    private void EnableRemainingElements()
    {
        for(int i = 0; i<elementalToggles.Length; i++)
        {
            elementalToggles[i].interactable = true;
        }
    }

    private void DisableAllChecks()
    {
        for(int i = 0; i<elementalToggles.Length; i++)
        {
            elementalToggles[i].isOn = false;
        } 
        swapButton.interactable = false;
    }

    /// <summary>
    /// Initiates the swap
    /// Changes both the rect transform and list location of the element
    /// </summary>
    public void Swap()
    {
        // find both selected toggles via index
        List<int> swapToggles = new List<int>();
        for (int i = 0; i < elementalToggles.Length; i++)
        {
            if (elementalToggles[i].isOn)
            {
                swapToggles.Add(i);
            }
        }

        // UI Location Swapping Shenannigans
        Vector3 tempPos0= elementalToggles[swapToggles[0]].GetComponent<RectTransform>().anchoredPosition;
        Vector3 tempPos1 = elementalToggles[swapToggles[1]].GetComponent<RectTransform>().anchoredPosition;
        
        elementalToggles[swapToggles[0]].gameObject.GetComponent<RectTransform>().anchoredPosition =
            tempPos1;
        
        elementalToggles[swapToggles[1]].gameObject.GetComponent<RectTransform>().anchoredPosition =
            tempPos0;

        //list swapping
        Toggle tempToggle = elementalToggles[swapToggles[0]];
        elementalToggles[swapToggles[0]] = elementalToggles[swapToggles[1]];
        elementalToggles[swapToggles[1]] = tempToggle;
        
        //Disable all checkmarks and swap button

        CreateListOrder();
        DisableAllChecks();
        
        
    }

    #endregion
}
