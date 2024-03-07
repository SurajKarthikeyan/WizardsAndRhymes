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
    [Tooltip("The current tape the player is on")]
    [SerializeField] private GameObject currentTape;
    [Tooltip("Low alpha setting for when a mixtape is not selected")]
    [SerializeField] private float lowAlpha;
    [Tooltip("High alpha setting for when a mixtape is selected")]
    [SerializeField] private float highAlpha;
    [Tooltip("List of inventory mixtape objects in scene")]
    [SerializeField] private List<GameObject> mixtapeInventory;

    [Tooltip("Index on which mixtape the character is on")]
    private int index = 0;
    
    [Tooltip("The damage type currently being used by the enemey")]
    [SerializeField] public Health.DamageType damageType;

    [Tooltip("Rect transforms for the 1st, 2nd and 3rd element slot locations")]
    [SerializeField] private List<RectTransform> slots;
    [Tooltip("String element passed here to use for order")]
    [HideInInspector] public List<String> inventoryOrder;
    
    [Header("Mixtape Object references")]
    [SerializeField] private GameObject fire;
    [SerializeField] private GameObject lightning;
    [SerializeField] private GameObject ice;

    [Tooltip("Reference to the player script")]
    [SerializeField] private PlayerController player;
    #endregion

    #region UnityMethods
    /// <summary>
    /// Ensures starting parameters are set and the correct starting damage is right
    /// </summary>
    private void Start()
    {
        currentTape = mixtapeInventory[index]; // default value
        ChangeColor(currentTape, highAlpha);
        damageType = mixtapeInventory[index].GetComponent<Mixtape>().mixtapeDType;  // set without playing sfx

    }

    #endregion

    #region CustomMethods

    /// <summary>
    /// Set the order of the mixtapes in scene
    /// </summary>
    public void SetOrder()
    {
        // Set list
        for (int i = 0; i < inventoryOrder.Count; i++)
        {
            String currentOrder = inventoryOrder[i];
            switch (currentOrder)
            {
                case "fire":
                    mixtapeInventory[i] = fire;
                    mixtapeInventory[i].GetComponent<RectTransform>().anchoredPosition = slots[i].anchoredPosition;
                    break;
                case "lightning":
                    mixtapeInventory[i] = lightning;
                    mixtapeInventory[i].GetComponent<RectTransform>().anchoredPosition = slots[i].anchoredPosition;
                    break;
                case "ice":
                    mixtapeInventory[i] = ice;
                    mixtapeInventory[i].GetComponent<RectTransform>().anchoredPosition = slots[i].anchoredPosition;
                    break;
            }
        }
    }
    
    
    /// <summary>
    /// Changes what mixtape is the "main" mixtape
    /// </summary>
    public void OnTapeChange(bool isRanged)
    {
        ChangeColor(currentTape, lowAlpha);
        damageType = mixtapeInventory[index].GetComponent<Mixtape>().GetDamagePlaySound(isRanged); // set WITH playing sfx
        int newIndex = index + 1;

        if (newIndex >= mixtapeInventory.Count)
        {
            player.ComboCoolDown();
        }
        index = (newIndex) % mixtapeInventory.Count;
        currentTape = mixtapeInventory[index];
        damageType = mixtapeInventory[index].GetComponent<Mixtape>().mixtapeDType;
        ChangeColor(currentTape, highAlpha);
    }

    /// <summary>
    /// Resets the combo
    /// </summary>
    public void ResetCombo()
    {
        ChangeColor(currentTape, lowAlpha);
        index = 0;
        damageType = mixtapeInventory[index].GetComponent<Mixtape>().mixtapeDType;  // reset without playing sounds
        currentTape = mixtapeInventory[index];
        ChangeColor(currentTape, highAlpha);
    }

    /// <summary>
    /// Changes Alpha of specific tape
    /// </summary>
    /// <param name="tape"></param>
    /// <param name="setAlpha"></param>
    public void ChangeColor(GameObject tape, float setAlpha)
    {
        tape.GetComponent<Image>().color = new Color(tape.GetComponent<Image>().color.r,
            tape.GetComponent<Image>().color.g, tape.GetComponent<Image>().color.b, setAlpha);
    }
    
    #endregion

}
