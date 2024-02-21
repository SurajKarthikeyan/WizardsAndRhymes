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
    #endregion

    #region UnityMethods
    /// <summary>
    /// Ensures starting parameters are set and the correct starting damage is rightd
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
    /// Changes what mixtape is the "main" mixtape
    /// </summary>
    public void OnTapeChange(bool isRanged)
    {
        ChangeColor(currentTape, lowAlpha);
        damageType = mixtapeInventory[index].GetComponent<Mixtape>().GetDamagePlaySound(isRanged); // set WITH playing sfx
        index = (index + 1) % mixtapeInventory.Count;
        currentTape = mixtapeInventory[index];
        ChangeColor(currentTape, highAlpha);
    }

    public void ResetCombo()
    {
        ChangeColor(currentTape, lowAlpha);
        index = 0;
        damageType = mixtapeInventory[index].GetComponent<Mixtape>().mixtapeDType;  // reset without playing sounds
        currentTape = mixtapeInventory[index];
        ChangeColor(currentTape, highAlpha);
    }

    public void ChangeColor(GameObject tape, float setAlpha)
    {
        tape.GetComponent<Image>().color = new Color(tape.GetComponent<Image>().color.r,
            tape.GetComponent<Image>().color.g, tape.GetComponent<Image>().color.b, setAlpha);
    }
    
    #endregion

}
