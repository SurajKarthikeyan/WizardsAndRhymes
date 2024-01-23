using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class contains the speaker who is speaking for a dialogue as well as an array of dialogue sentences
/// which are spoken by the speaker when the dialogue is played
/// </summary>
[System.Serializable]
public class Dialogue
{
    [Tooltip("The character which is speaking this dialogue")]
    public Speaker speaker;
    [Tooltip("The dialogue strings that are being spoken by the speaker")]
    public DialogueSentence[] dialogueSentences;
}
