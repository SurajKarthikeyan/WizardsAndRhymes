using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WordCanvasManager : MonoBehaviour
{
    public bool canvasActivated;

    public Canvas wordCanvas;

    public List<DragDropItemSlot> wordSlots;

    public bool validWords;

    private void Start()
    {
        foreach (var wordSlot in wordSlots)
        {
            wordSlot.manager = this;
        }
    }

    public bool ValidateCanvas()
    {
        foreach (var wordSlot in wordSlots)
        {
            if (!wordSlot.correctSlot)
            {
                return false;
            }
        }

        Debug.Log("words are in proper spot");
        return true;
    }


    public void ToggleCanvas(bool canvasOn)
    {

        canvasActivated = canvasOn;
        wordCanvas.gameObject.SetActive(canvasActivated);
        
        if (canvasActivated)
        {
            PlayerController.instance.DisablePlayerControls();
        }
        else
        {
            PlayerController.instance.EnablePlayerControls();
        }
    }
}
