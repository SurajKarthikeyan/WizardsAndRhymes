using AYellowpaper.SerializedCollections;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public List<BaseEnemy> enemies;

    /// <summary>
    /// Dictionary that holds the names of the plant prefabs and a list of strings used for their text
    /// </summary>
    [Tooltip("Dictionary that contains a mapping of prefab names and dialogue text")]
    [SerializedDictionary("Plant Prefab Name", "Informative Text")]
    public SerializedDictionary<string, List<string>> plantTextPairs;

    /// <summary>
    /// Dialogue holder reference
    /// </summary>
    [Tooltip("Reference to the DialogueHolder")]
    public DialogueHolder dialogueHolder;

    /// <summary>
    /// Dialogue held within the dialogue holder
    /// </summary>
    private Dialogue dialogue;

    // Start is called before the first frame update
    void Start()
    {
        enemies = FindObjectsOfType<BaseEnemy>().ToList();
    }

    public void ActivateEnemies()
    {
        foreach (BaseEnemy enemy in enemies)
        {
            enemy.GetComponent<MeshRenderer>().material = enemy.m_ActivatedMaterial;
            enemy.m_Activated = true;
        }
        
    }

    /// <summary>
    /// Updates the dialogue from the dictionary
    /// </summary>
    /// <param name="plantName">Name of the plant prefab</param>
    public void UpdatePlantDialogue(string plantName)
    {
        //If the dictionary contains the name of the plant, we copy the text from the dictionary into the dialogue
        if (plantTextPairs.ContainsKey(plantName))
        {
            List<string> dialogueStrings = plantTextPairs[plantName];
            dialogue.dialogueSentences = new DialogueSentence[dialogueStrings.Count];

            for (int i = 0; i < dialogue.dialogueSentences.Length; i++)
            {
                DialogueSentence currentDialogueSentence = new()
                {
                    sentence = dialogueStrings[i]
                };
                dialogue.dialogueSentences[i] = currentDialogueSentence;
            }
        }
        else
        {
            dialogueHolder.dialogueToPlayBack.Clear();
        }
    }
}
