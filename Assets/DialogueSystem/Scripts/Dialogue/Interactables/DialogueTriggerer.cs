using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class allows dialogue to be called when the player moves into a trigger in the world
/// </summary>
public class DialogueTriggerer : MonoBehaviour
{
    [Tooltip("The Dialogue Holder whose dialogue will be played back once the player enters the trigger")]
    public DialogueHolder dialogueHolder;
    [Tooltip("Whether or not this dialogue trigger should only happen once")]
    public bool playOnce = false;
    [Tooltip("Whether or not this trigger has already played its dialogue")]
    public bool alreadyPlayed = false;
    [Tooltip("Whether or not to set this gameobject to inactive after playing the dialogue")]
    public bool setToInactiveAfterPlay = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (playOnce == true && alreadyPlayed == true)
            {
                return;
            }
            else
            {
                alreadyPlayed = true;
                dialogueHolder.TriggerDialogue();
            }

            if(setToInactiveAfterPlay)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
