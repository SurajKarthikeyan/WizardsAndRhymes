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
    private void Start()
    {
        currentTape = mixtapeInventory[index]; // default value
        ChangeColor(currentTape, highAlpha);
    }

    public void OnTapeChange()
    {
        Debug.Log("TapeChange");
        ChangeColor(currentTape, lowAlpha);
        index = (index + 1) % mixtapeInventory.Count;
        currentTape = mixtapeInventory[index];
        ChangeColor(currentTape, highAlpha);
    }

    public void ChangeColor(GameObject tape, float setAlpha)
    {
        tape.GetComponent<Image>().color = new Color(tape.GetComponent<Image>().color.r,
            tape.GetComponent<Image>().color.g, tape.GetComponent<Image>().color.b, setAlpha);
    }
}
