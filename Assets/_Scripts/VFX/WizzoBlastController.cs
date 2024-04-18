using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using MoreMountains.Feedbacks;

/// <summary>
/// Handles the behaviour of the Wizzo Blast sequence
/// </summary>
public class WizzoBlastController : MonoBehaviour
{
    [Tooltip("The MMF_Player to play feedbacks from when interact is pressed")]
    [SerializeField] MMF_Player interactFeedbacks;
    [Tooltip("How many times the player has to press interact to charge the blast")]
    [SerializeField] int blastChargedCount = 10;

    [Tooltip("The PlayerInput instance used to detect input")]
    PlayerInput playerInput;
    [Tooltip("The number of times the player has pressed interact")]
    int interactCount;
    [Tooltip("Whether the blast has started")]
    bool blastStarted = false;

    /// <summary>
    /// Initialization
    /// </summary>
    private void Awake()
    {
        //Initialize input
        playerInput = new PlayerInput();
        playerInput.Player.Enable();
        playerInput.Player.Interact.started += DoInteract;
    }

    /// <summary>
    /// Called whenever the interact button is pressed
    /// </summary>
    /// <param name="obj">The input action callback context</param>
    private void DoInteract(InputAction.CallbackContext obj)
    {
        //Ignore if the blast has already started
        if (blastStarted)
            return;

        Debug.Log("Interacted");
        interactCount++;

        interactFeedbacks.PlayFeedbacks();

        if (interactCount >= blastChargedCount)
        {
            Debug.Log("Blast started");
            blastStarted = true;
        }
    }
}
