using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// This class contatins the sentence to be spoken and also contains functions and settings for when the specific dialogue stirng is read out by the dialogue manager
/// </summary>
[System.Serializable]
public class DialogueSentence
{
    public string sentence;
    public bool leaveAtEndOfSentence = false;

    [HideInInspector]
    public Speaker.Position dialgoueSpritePosition;

    public UnityEvent eventsWhenStartingDialogueString;
    public UnityEvent eventsWhenFinishingDialogueString;

    public void DoWhenStartingDialogueString()
    {
        eventsWhenStartingDialogueString.Invoke();
    }

    public void DoWhenEndingDialogueString()
    {
        if (leaveAtEndOfSentence)
        {
            if (dialgoueSpritePosition == Speaker.Position.Left)
            {
                DialogueManager.instance.leftSprite.ResetSprite();
            }
            else if (dialgoueSpritePosition == Speaker.Position.Right)
            {
                DialogueManager.instance.rightSprite.ResetSprite();
            }
        }
        eventsWhenFinishingDialogueString.Invoke();
    }
}
