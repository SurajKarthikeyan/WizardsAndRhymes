using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueHolder : MonoBehaviour
{
    [Header("Dialogue Settings")]
    [Tooltip("The dialogues to play back")]
    public List<Dialogue> dialogueToPlayBack;

    [Header("Play Settings")]
    [Tooltip("Whether or not to pause when it occurs")]
    public bool pauseOnTrigger = true;
    [Tooltip("Whether or not to play this dialogue when the script starts")]
    public bool playOnStart = false;

    private void Start()
    {
        if (playOnStart)
        {
            TriggerDialogue();
        }
    }

    /// <summary>
    /// Triggers the dialogue and gets it playing
    /// </summary>
    public void TriggerDialogue()
    {
        SetUpDialogue();
    }

    /// <summary>
    /// Sets up the dialogue and starts it, will pause the game if pause on trigger is set to true.
    /// </summary>
    void SetUpDialogue()
    {
        DialogueManager.instance.StartDialogue(dialogueToPlayBack);

        if (pauseOnTrigger)
        {
            Time.timeScale = 0;
        }
    }
}
