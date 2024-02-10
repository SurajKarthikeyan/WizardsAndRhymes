using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MixtapeInventory : MonoBehaviour
{
    public List<GameObject> mixtapeInventory;
    public int index = 0;
    public GameObject currentTape;
    public float lowAlpha;
    public float highAlpha;
    public Health.DamageType damageType;
    private void Start()
    {
        currentTape = mixtapeInventory[index]; // default value
        ChangeColor(currentTape, highAlpha);
        SetDamageType(index);
    }

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
}
