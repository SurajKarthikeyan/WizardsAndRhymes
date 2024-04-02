using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Class that handles the word canvas 
/// </summary>
public class WordCanvasManager : MonoBehaviour
{
    [Tooltip("Boolean stating whether or not the canvas is activated")]
    private bool canvasActivated;

    [Tooltip("Canvas holding the words")]
    [SerializeField]
    private Canvas wordCanvas;

    [Tooltip("List of all word slots")]
    [SerializeField]
    private List<DragDropItemSlot> wordSlots;

    /// <summary>
    /// Function that is called on scene start
    /// </summary>
    private void Start()
    {
        foreach (var wordSlot in wordSlots)
        {
            wordSlot.manager = this;
        }
    }

    /// <summary>
    /// Function that is used to validate this word canvas, seeing if all the slots have the correct word
    /// </summary>
    /// <returns></returns>
    public bool ValidateCanvas()
    {
        foreach (var wordSlot in wordSlots)
        {
            if (!wordSlot.correctSlot)
            {
                return false;
            }
        }

        GameEnd.gameEnd.OpenDoorUI();
        wordCanvas.gameObject.SetActive(false);
        return true;
    }

    /// <summary>
    /// Toggles the canvas visibility on and off
    /// </summary>
    /// <param name="canvasOn">Boolean stating whether or not the canvas should be visible</param>
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