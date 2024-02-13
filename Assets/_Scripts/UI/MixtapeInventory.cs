using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    private void Start()
    {
        currentTape = mixtapeInventory[index]; // default value
        ChangeColor(currentTape, highAlpha);
        SetDamageType(index);
    }
    #endregion

    #region CustomMethods
    public void OnTapeChange()
    {
        ChangeColor(currentTape, lowAlpha);
        index = (index + 1) % mixtapeInventory.Count;
        SetDamageType(index);
        currentTape = mixtapeInventory[index];
        ChangeColor(currentTape, highAlpha);
    }

    public void ChangeColor(GameObject tape, float setAlpha)
    {
        tape.GetComponent<Image>().color = new Color(tape.GetComponent<Image>().color.r,
            tape.GetComponent<Image>().color.g, tape.GetComponent<Image>().color.b, setAlpha);
    }

    /// <summary>
    /// Temporary - setup a system that can set to any element based on object, not index(that can change eventually).
    /// </summary>
    /// <param name="index"></param>
    public void SetDamageType(int index)
    {
        if (index == 0)
        {
            damageType = Health.DamageType.Fire;
        }
        else if (index == 1)
        {
            damageType = Health.DamageType.Lightning;
        }
        else
        {
            damageType = Health.DamageType.Ice;
        }
    }
    #endregion

}
