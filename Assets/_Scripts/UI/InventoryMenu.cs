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
    [SerializeField] private Toggle[] elementalToggles;
    [SerializeField] private int toggleCount;
    [SerializeField] private Button swapButton;
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
        mixtapeInventory.SetOrder();

    }
    #endregion

    #region CustomMethods

    public void CreateListOrder()
    {
        List<String> sendList = new List<String>();
        for (int i = 0; i < elementalToggles.Length; i++)
        {
            sendList.Add(elementalToggles[i].name);
        }
        mixtapeInventory.inventoryOrder = sendList;
    }

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
