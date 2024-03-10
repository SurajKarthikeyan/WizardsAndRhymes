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
    
    [Tooltip("The damage type currently being used by the enemey")]
    [SerializeField] public Health.DamageType damageType;
    

    [Tooltip("Reference to the player script")]
    [SerializeField] private PlayerController player;

    public int successiveAttacks;
    #endregion

    #region UnityMethods
    /// <summary>
    /// Ensures starting parameters are set and the correct starting damage is right
    /// </summary>
    private void Start()
    {
        damageType = mixtapeInventory[index].GetComponent<Mixtape>().mixtapeDType;  // set without playing sfx
    }

    #endregion

    #region CustomMethods
    
    
    
    /// <summary>
    /// Changes what mixtape is the "main" mixtape
    /// </summary>
    public void OnTapeChange(bool isRanged)
    {
        damageType = mixtapeInventory[index].GetComponent<Mixtape>().GetDamagePlaySound(isRanged); // set WITH playing sfx
        int newIndex = index + 1;
        if (newIndex >= mixtapeInventory.Count)
        {
            player.ComboCoolDown();
        }
        index = (newIndex) % mixtapeInventory.Count;
        damageType = mixtapeInventory[index].GetComponent<Mixtape>().mixtapeDType;
    }

    /// <summary>
    /// Resets the combo
    /// </summary>
    public void ResetCombo()
    {
        successiveAttacks = 0;
        index = 0;
        damageType = mixtapeInventory[index].GetComponent<Mixtape>().mixtapeDType;  // reset without playing sounds
    }

    public void IncrementSuccessiveAttack()
    {
        successiveAttacks++;
    }
    
    
    #endregion

}
