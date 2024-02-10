using System;
using AYellowpaper.SerializedCollections;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Class that manages all the enemies currently in the scene
/// </summary>
public class EnemyManager : MonoBehaviour
{
    #region Variables
    [Tooltip("Singleton of the enemy manager - seemed useful to make")]
    public static EnemyManager enemyManager;
    
    [Tooltip("List of enemies that are active in scene")]
    [SerializeField] public List<BaseEnemyBehavior> enemies;
    #endregion

    ///// <summary>
    ///// Dictionary that holds the names of the plant prefabs and a list of strings used for their text
    ///// </summary>
    //[Tooltip("Dictionary that contains a mapping of prefab names and dialogue text")]
    //[SerializedDictionary("Plant Prefab Name", "Informative Text")]
    //public SerializedDictionary<string, List<string>> plantTextPairs;

    ///// <summary>
    ///// Dialogue holder reference
    ///// </summary>
    //[Tooltip("Reference to the DialogueHolder")]
    //public DialogueHolder dialogueHolder;

    ///// <summary>
    ///// Dialogue held within the dialogue holder
    ///// </summary>
    //private Dialogue dialogue;

    #region Unity Methods

    /// <summary>
    /// Method called on the first scene of the game
    /// </summary>
    private void Awake()
    {
        if (enemyManager == null)
        {
            enemyManager = this;
        }
        else
        {
            Debug.LogError("You have two EnemyManagers in Scene, remove one");
        }
    }


    /// <summary>
    /// Method called on scene start
    /// </summary>
    void Start()
    {
        enemies = FindObjectsOfType<BaseEnemyBehavior>().ToList();
    }
    #endregion

    #region Custom Methods
    /// <summary>
    /// Function that activates the enemies in the scene
    /// </summary>
    public void ActivateEnemies()
    {
        foreach (BaseEnemyBehavior enemy in enemies)
        {
            enemy.GetComponent<MeshRenderer>().material = enemy.activatedMaterial;
            enemy.activated = true;
        }
        
    }

    /// <summary>
    /// THIS IS TEMPORARILY HERE UNTIL I FIND SOMEWHERE TO PUT IT - Zane
    /// </summary>
    /// <param name="plantName">Name of the plant prefab</param>
    //private void UpdatePlantDialogue(string plantName)
    //{
    //    //If the dictionary contains the name of the plant, we copy the text from the dictionary into the dialogue
    //    if (plantTextPairs.ContainsKey(plantName))
    //    {
    //        List<string> dialogueStrings = plantTextPairs[plantName];
    //        dialogue.dialogueSentences = new DialogueSentence[dialogueStrings.Count];

    //        for (int i = 0; i < dialogue.dialogueSentences.Length; i++)
    //        {
    //            DialogueSentence currentDialogueSentence = new()
    //            {
    //                sentence = dialogueStrings[i]
    //            };
    //            dialogue.dialogueSentences[i] = currentDialogueSentence;
    //        }
    //    }
    //    else
    //    {
    //        dialogueHolder.dialogueToPlayBack.Clear();
    //    }
    //}
    #endregion
}
