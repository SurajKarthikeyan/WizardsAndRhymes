using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Class that handles the word canvas 
/// </summary>
public class WordCanvasManager : MonoBehaviour, IInteractable
{
    [Tooltip("Boolean stating whether or not the canvas is activated")]
    private bool canvasActivated;

    [Tooltip("Canvas holding the words")]
    [SerializeField]
    private Canvas wordCanvas;

    [Tooltip("List of all word slots")]
    [SerializeField]
    private List<DragDropItemSlot> wordSlots;

    [Tooltip("List of all word slots")]
    [SerializeField]
    private List<DragDropItem> words;

    public PauseMenu pauseMenu;
    
    public GameObject wordParent;

    [SerializeField] private AK.Wwise.Event rapRockESoundEffect;

    /// <summary>
    /// Function that is called on scene start
    /// </summary>
    private void Start()
    {
        foreach (var wordSlot in wordSlots)
        {
            wordSlot.manager = this;
        }
        foreach (var word in words)
        {
            /*word.startPosition = word.transform.position;*/
            word.gameObject.SetActive(false);
        }
        wordCanvas.gameObject.SetActive(false);
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
        wordCanvas.gameObject.SetActive(false);
        wordParent.SetActive(false);
        pauseMenu.SceneCompleteUI();
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
        
        if (FlagManager.instance.GetFlag("word1") && !words[0].gameObject.activeInHierarchy)
        {
            words[0].gameObject.SetActive(true);
        }
        if (FlagManager.instance.GetFlag("word2") && !words[1].gameObject.activeInHierarchy)
        {
            words[1].gameObject.SetActive(true);
        }
        if (FlagManager.instance.GetFlag("word3") && !words[2].gameObject.activeInHierarchy)
        {
            words[2].gameObject.SetActive(true);
        }
        
        if (canvasActivated)
        {
            foreach (var word in words)
            {
                /*word.transform.position = word.startPosition;*/
                word.canMove = true;
            }
            PlayerController.instance.DisablePlayerControls();
        }
        else
        {
            PlayerController.instance.EnablePlayerControls();
        }
    }

    public void Interact()
    {
        if (!canvasActivated)
        {
            ToggleCanvas(true);
            wordParent.SetActive(false);
            rapRockESoundEffect.Post(this.gameObject);
        }
        else
        {
            ToggleCanvas(false);
            wordParent.SetActive(true);
        }
    }
}
