using System;
using AYellowpaper.SerializedCollections;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using MoreMountains.Tools;
using Unity.VisualScripting;

/// <summary>
/// Class that manages waves of enemies for the current room
/// </summary>
public class EnemyManager : MonoBehaviour
{
    #region Variables
    [Tooltip("Singleton of the enemy manager - seemed useful to make")]
    public static EnemyManager instance;

    /// <summary>
    /// Class that describes a wave of enemies
    /// </summary>
    [Serializable]
    class EnemyWave
    {
        [Tooltip("Potential ways for this wave to trigger")]
        public enum WaveTrigger { EnemiesRemaining, WithPreviousWave, PreviousWaveDoneSpawning }


        [Tooltip("When to trigger this wave (doesn't affect first wave)")]
        public WaveTrigger waveTrigger = WaveTrigger.EnemiesRemaining;
        [Tooltip("The number of enemies remaining in this scene before this wave spawns")]
        [MMEnumCondition("waveTrigger", (int)WaveTrigger.EnemiesRemaining, Hidden = true)]
        public float enemiesRemaining = 0;
        [Tooltip("The delay from when this wave is triggered to when the first enemy spawns")]
        public float waveDelay = 0;
        [Tooltip("The time from when the first enemy spawns to when the last enemy spawns")]
        public float waveDuration;
        [Tooltip("The transform to spawn the enemies at the position of")]
        public Transform spawnPosition;
        [Tooltip("The radius around the spawn point in which enemies can spawn")]
        public float spawnRadius;
        [Tooltip("Whether to display the spawn sphere for this wave in edit mode")]
        public bool showSpawnSphere;
        [Tooltip("The enemy prefabs to spawn")]
        public EnemyWaveGroup[] enemies;
    }

    /// <summary>
    /// Class that represents a group of a single type of enemy within a wave (serialize dictionaries don't work inside a nested class)
    /// </summary>
    [Serializable]
    class EnemyWaveGroup
    {
        [Tooltip("The enemy prefab this is a group of")]
        public GameObject type;
        [Tooltip("The number of this enemy in the group")]
        public int amount;
    }

    public delegate void RoomClearedDelegate();
    [Tooltip("Event fired when the all waves of enemies have been cleared")]
    [HideInInspector] public static event RoomClearedDelegate roomCleared;

    [Tooltip("Array of enemy waves to spawn")]
    [SerializeField] EnemyWave[] waves = new EnemyWave[] { new EnemyWave() };

    [Tooltip("The index of the next wave of enemies")]
    [SerializeField] int waveIndex = 0;
    [Tooltip("The number of enemies, spawned and yet to be spawned, remaining in the current waves")]
    [SerializeField] int enemiesRemaining = 0;
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
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogError("You have two EnemyManagers in Scene, remove one");
            Destroy(this);
        }

        //DEBUG Activate spawning immediately
        ActivateEnemies();
    }

    /// <summary>
    /// Method called to draw debug overlays in edit mode
    /// </summary>
    private void OnDrawGizmos()
    {
        //Draw the spawn spheres for each wave
        foreach (EnemyWave wave in waves)
        {
            if (wave.showSpawnSphere)
            {
                Gizmos.DrawWireSphere(wave.spawnPosition.position, wave.spawnRadius);
            }
        }
    }
    #endregion

    #region Custom Methods
    /// <summary>
    /// Function that activates the enemies in the scene
    /// </summary>
    public void ActivateEnemies()
    {
        //Spawn the first wave of enemies
        if (waves.Length > 0)
            StartCoroutine(TriggerWave(waves[0]));
    }

    /// <summary>
    /// Called by the base enemy health script whenever an enemy dies
    /// </summary>
    public void EnemyDied()
    {
        enemiesRemaining--;

        //Check if the next wave should spawn now
        if (waveIndex < waves.Length)
        {
            EnemyWave currentWave = waves[waveIndex];
            if (currentWave.waveTrigger == EnemyWave.WaveTrigger.EnemiesRemaining)
            {
                if (enemiesRemaining <= currentWave.enemiesRemaining)
                    StartCoroutine(TriggerWave(currentWave));
            }
        }
        //Check if the last wave has been defeated
        else if (enemiesRemaining <= 0)
        {
            roomCleared?.Invoke(); //Invoke the room cleared event
        }
    }

    /// <summary>
    /// Triggers the spawning of a wave of enemies
    /// </summary>
    /// <param name="wave">The wave of enemies to trigger</param>
    /// <returns>Coroutine</returns>
    IEnumerator TriggerWave(EnemyWave wave)
    {
        //Wait for this wave's delay
        yield return new WaitForSeconds(wave.waveDelay);

        //Increment the wave index
        int thisWaveIndex = waveIndex;
        waveIndex++;

        //Check if the next wave should spawn alongside this one
        if (waveIndex < waves.Length)
        {
            EnemyWave nextWave = waves[waveIndex];
            if (nextWave.waveTrigger == EnemyWave.WaveTrigger.WithPreviousWave)
                StartCoroutine(TriggerWave(nextWave));
        }

        //Calculate the total number of enemies in this wave
        int waveEnemyCount = 0;
        foreach (EnemyWaveGroup waveGroup in wave.enemies)
            waveEnemyCount += waveGroup.amount;

        //Increase the enemies remaining count to account for this wave
        enemiesRemaining += waveEnemyCount;

        //Calculate the delay there should be between spawning each enemy
        float enemySpawnDelay = wave.waveDuration / waveEnemyCount;

        //Spawn the enemies
        foreach (EnemyWaveGroup waveGroup in wave.enemies)
        {
            GameObject enemyPrefab = waveGroup.type;

            for (int i = 0; i<waveGroup.amount; i++)
            {
                Vector2 spawnPosition2D = UnityEngine.Random.insideUnitCircle * wave.spawnRadius;
                Vector3 spawnPosition = new Vector3(wave.spawnPosition.position.x + spawnPosition2D.x, wave.spawnPosition.position.y, wave.spawnPosition.position.z + spawnPosition2D.y);
                GameObject enemyGO = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
                enemyGO.GetComponent<BaseEnemyBehavior>().activated = true; //Activate the enemy
                yield return new WaitForSeconds(enemySpawnDelay);
            }
        }

        //Check if the next wave should spawn when this wave is done spawning
        if (waveIndex < waves.Length && waveIndex == thisWaveIndex + 1)
        {
            EnemyWave nextWave = waves[waveIndex];
            if (nextWave.waveTrigger == EnemyWave.WaveTrigger.PreviousWaveDoneSpawning)
                StartCoroutine(TriggerWave(nextWave));
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
