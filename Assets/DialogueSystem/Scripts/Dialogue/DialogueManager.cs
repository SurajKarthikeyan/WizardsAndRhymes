using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DialogueManager : MonoBehaviour
{
    [Header("Dialogue Box Settings")]
    public Text nameText;
    public Text sentenceDisplay;
    private Queue<DialogueSentence> currentDialogueStrings;

    public static DialogueManager instance;

    public Animator animator;

    [Header("Character Representation Settings")]
    public DialogueSprite leftSprite;
    public DialogueSprite rightSprite;

    Task runningTextDisplayTask;
    DialogueSentence currentDialogueString;

    // left side is 0 right side is 1
    public int activeSpeaker = 0;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
        currentDialogueStrings = new Queue<DialogueSentence>();

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    [Header("Dialogue State Information")]
#pragma warning disable CS0414
    public bool dialogueRunning = false;
#pragma warning restore CS0414
    private void Update()
    {

    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        EndDialogueWithoutUnPausing();
    }

    public void StartDialogue(Dialogue dialogue)
    {
        animator.SetBool("IsOpen", true);

        nameText.text = dialogue.speaker.speakerName;

        currentDialogueStrings.Clear();

        SetAndAnimateSprites(dialogue);

        foreach (DialogueSentence dialogueString in dialogue.dialogueSentences)
        {
            currentDialogueStrings.Enqueue(dialogueString);
        }
        dialogueRunning = true;

        DisplayNextSentence();
    }

    /// <summary>
    /// Sets the sprite on the left or right to be the dialogues sprite, removing the inactive speaker and setting the other speaker to inactive
    /// while setting the current dialogue to be the active speaker
    /// </summary>
    /// <param name="dialogue"></param>
    void SetAndAnimateSprites(Dialogue dialogue)
    {
        // See if the name already matches
        if (leftSprite.currentCharacterName == dialogue.speaker.speakerName)
        {
            leftSprite.MakeActiveSprite(dialogue);
            foreach(DialogueSentence dialogueString in dialogue.dialogueSentences)
            {
                dialogueString.dialgoueSpritePosition = Speaker.Position.Left;
            }
            rightSprite.MakeInactiveSprite();
        }
        else if (rightSprite.currentCharacterName == dialogue.speaker.speakerName)
        {
            rightSprite.MakeActiveSprite(dialogue);
            foreach (DialogueSentence dialogueString in dialogue.dialogueSentences)
            {
                dialogueString.dialgoueSpritePosition = Speaker.Position.Right;
            }
            leftSprite.MakeInactiveSprite();
        }
        // If none of the names already match then go by whichever is not active
        else if (leftSprite.CheckIfActive())
        {
            rightSprite.MakeActiveSprite(dialogue);
            foreach (DialogueSentence dialogueString in dialogue.dialogueSentences)
            {
                dialogueString.dialgoueSpritePosition = Speaker.Position.Right;
            }
            leftSprite.MakeInactiveSprite();
        }
        else if (rightSprite.CheckIfActive())
        {
            leftSprite.MakeActiveSprite(dialogue);
            foreach (DialogueSentence dialogueString in dialogue.dialogueSentences)
            {
                dialogueString.dialgoueSpritePosition = Speaker.Position.Left;
            }
            rightSprite.MakeInactiveSprite();
        }
        // If neither is active, default to their preferred position
        else if (dialogue.speaker.preferredPosition == Speaker.Position.Right)
        {
            rightSprite.MakeActiveSprite(dialogue);
            foreach (DialogueSentence dialogueString in dialogue.dialogueSentences)
            {
                dialogueString.dialgoueSpritePosition = Speaker.Position.Right;
            }
            leftSprite.MakeInactiveSprite();
        }
        else
        {
            leftSprite.MakeActiveSprite(dialogue);
            foreach (DialogueSentence dialogueString in dialogue.dialogueSentences)
            {
                dialogueString.dialgoueSpritePosition = Speaker.Position.Left;
            }
            rightSprite.MakeInactiveSprite();
        }

    }

    int dialogueIndex = 0;
    bool listRunning = false;
    List<Dialogue> currentList;
    public void StartDialogue(List<Dialogue> dialogueList)
    {
        dialogueIndex = 0;
        listRunning = true;
        currentList = dialogueList;
        StartDialogue(currentList[0]);
    }

    IEnumerator TypeSentence (string sentence)
    {
        sentenceDisplay.text = "";
        currentDialogueString.DoWhenStartingDialogueString();
        foreach (char letter in sentence.ToCharArray())
        {
            sentenceDisplay.text += letter;
            yield return null;
        }
    }

    public void DisplayNextSentence()
    {
        if (runningTextDisplayTask != null && runningTextDisplayTask.Running)
        {
            sentenceDisplay.text = currentDialogueString.sentence;
            runningTextDisplayTask.Stop();
            return;
        }

        if (currentDialogueStrings.Count <= 0 && listRunning == false)
        {
            EndDialogue();
            return;
        }
        else if (currentDialogueStrings.Count <= 0 && listRunning == true)
        {
            dialogueIndex += 1;
            if (dialogueIndex >= currentList.Count)
            {
                EndDialogue();
                return;
            }
            else
            {
                animator.SetBool("IsOpen", false);
                StartDialogue(currentList[dialogueIndex]);
                return;
            }
        }

        if (currentDialogueString != null)
        {
            currentDialogueString.DoWhenEndingDialogueString();
        }
        currentDialogueString = currentDialogueStrings.Dequeue();
        if (runningTextDisplayTask != null)
        {
            runningTextDisplayTask.Stop();
        }
        runningTextDisplayTask = new Task(TypeSentence(currentDialogueString.sentence));

    }

    public void EndDialogue()
    {
        animator.SetBool("IsOpen", false);
        leftSprite.ResetSprite();
        rightSprite.ResetSprite();
        if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
        }
        dialogueRunning = false;
        currentDialogueString.DoWhenEndingDialogueString();
        currentDialogueString = null;
    }

    public void EndDialogueWithoutUnPausing()
    {
        if (animator != null)
        {
            animator.SetBool("IsOpen", false);
            leftSprite.ResetSprite();
            rightSprite.ResetSprite();
        }
        if (currentList != null)
        {
            currentList.Clear();
        }
        currentDialogueString = null;
    }
}
