using System;
using AYellowpaper.SerializedCollections;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using MoreMountains.Tools;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.UI;
using static Health;
using UnityEngine.Events;

/// <summary>
/// Class that manages waves of enemies for the current room
/// </summary>
public class EnemyManager : MonoBehaviour
{
    #region Variables
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

    public delegate void WavesClearedDelegate(EnemyManager enemyManager);
    [Tooltip("Event fired when all waves of enemies from this enemy manager have been cleared")]
    [HideInInspector] public static event WavesClearedDelegate WavesCleared;

    public delegate void RoomClearedDelegate();
    [Tooltip("Event fired when the all waves of enemies from all enemy managers in the scene have been cleared")]
    [HideInInspector] public static event RoomClearedDelegate RoomCleared;

    public delegate void ActivateEnemiesDelegate(bool activateEnemies);
    [Tooltip("Event fired when debug button is pressed")]
    [HideInInspector] public static event ActivateEnemiesDelegate EnemiesActivated;

    [Tooltip("The number of enemy managers that haven't had all their waves cleared yet")]
    static int remainingEnemyManagers = 0;

    [Header("Call the function ActivateEnemies() using an event or script to start enemy spawning.")]
    [Tooltip("Array of enemy waves to spawn")]
    [SerializeField] EnemyWave[] waves = new EnemyWave[] { new EnemyWave() };
    [Tooltip("The spotlight prefab to spawn alongside each enemy")]
    [SerializeField] GameObject spotlightPrefab;
    [Tooltip("The events to trigger when all waves from this EnemyManager have been cleared")]
    [SerializeField] UnityEvent wavesClearedEvent;

    [Tooltip("Whether all of this enemy manager's waves have been completed")]
    [HideInInspector] public bool wavesCleared = false;

    [Tooltip("String of flag to set when this enemy manager's waves have been defeated")]
    [SerializeField] private string enemyManagerClearedEvent;

    [Tooltip("Whether enemy spawning has started")]
    bool spawningStarted = false;
    [Tooltip("The index of the next wave of enemies")]
    int waveIndex = 0;
    [Tooltip("The number of enemies, spawned and yet to be spawned, remaining in the current waves")]
    int enemiesRemainingInWave = 0;
    [Tooltip("A set of all the enemies that have been spawned")]
    HashSet<GameObject> enemiesSpawned = new HashSet<GameObject>();
    [Tooltip("The total number of enemies this EnemyManager is capable of spawning")]
    public int TotalEnemies { get; private set; } = 0;
    [Tooltip("The remaining number of enemies from this EnemyManager that are unspawned or active but not killed")]
    public int RemainingEnemies { get; private set; } = 0;

    [Header("Enemy Augmentation Variables")]

    [Tooltip("Current augmentation that will be applied to the enemies")]
    private EnemyAugmentation currentAugment;

    [Tooltip("Delegate that will be called to augment the enemy")]
    private delegate bool EnemyAugmentDelegate(GameObject enemyGO);

    [Tooltip("Instance of the augment enemy delegate")]
    private EnemyAugmentDelegate augmentEnemy;

    [Header("Debug tool variables")]
    [Tooltip("Toggle used to toggle enemy behavior")]
    [SerializeField]
    private Toggle enemyToggle;
    #endregion

    #region Unity Methods
    /// <summary>
    /// Register callbacks
    /// </summary>
    private void Start()
    {
        //Add self to count of enemy managers without all waves cleared
        remainingEnemyManagers += 1;

        //Calculate the total number of enemies this EnemyManager can spawn
        TotalEnemies = 0;
        foreach (EnemyWave wave in waves)
        {
            foreach (EnemyWaveGroup waveGroup in wave.enemies)
            {
                TotalEnemies += waveGroup.amount;
            }
        }
        RemainingEnemies = TotalEnemies;

        //Register enemy died callback
        BaseEnemyHealth.EnemyDied += EnemyDied;

        //Check whether this enemy manager has already been cleared
        if (FlagManager.instance.GetFlag(enemyManagerClearedEvent))
            ClearAllWaves();
    }

    /// <summary>
    /// Unregister callbacks
    /// </summary>
    private void OnDestroy()
    {
        //Remove self from count of enemy managers if still has uncleared waves
        if (!wavesCleared)
            remainingEnemyManagers -= 1;

        //Unregister enemy died callback
        BaseEnemyHealth.EnemyDied -= EnemyDied;
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
    [ContextMenu("Activate Enemies")]
    public void ActivateEnemies()
    {
        //Spawn the first wave of enemies
        if (waves.Length > 0 && !spawningStarted)
        {
            spawningStarted = true;
            StartCoroutine(TriggerWave(waves[0]));
        }
    }

    /// <summary>
    /// Instantly clears all of this enemy manager's waves
    /// </summary>
    [ContextMenu("Clear All Waves")]
    public void ClearAllWaves()
    {
        //If this enemy manager has already had all its waves cleared, skip
        if (wavesCleared)
            return;

        enemiesRemainingInWave = 0;
        RemainingEnemies = 0;

        AllWavesCleared();
        DestroyAllEnemies();
        StopCoroutine(nameof(TriggerWave));
    }

    /// <summary>
    /// Destroys all currently-active enemies this enemy manager has spawned
    /// </summary>
    [ContextMenu("Destroy All Enemies")]
    public void DestroyAllEnemies()
    {
        foreach (GameObject enemy in enemiesSpawned)
            Destroy(enemy);
        enemiesSpawned.Clear();
    }

    /// <summary>
    /// Function that deactivates the enemies in the scene
    /// </summary>
    public void ToggleEnemies()
    {
        EnemiesActivated?.Invoke(enemyToggle.isOn);
    }

    /// <summary>
    /// Called by the base enemy health script whenever an enemy dies
    /// </summary>
    public void EnemyDied(GameObject enemyGO)
    {
        //Check if this enemy was spawned by this EnemyManager
        if (enemiesSpawned.Contains(enemyGO))
        {
            enemiesSpawned.Remove(enemyGO);
            enemiesRemainingInWave--;
            RemainingEnemies--;

            //Check if the next wave should spawn now
            if (waveIndex < waves.Length)
            {
                EnemyWave currentWave = waves[waveIndex];
                if (currentWave.waveTrigger == EnemyWave.WaveTrigger.EnemiesRemaining)
                {
                    if (enemiesRemainingInWave <= currentWave.enemiesRemaining)
                        StartCoroutine(TriggerWave(currentWave));
                }
            }
            //Check if the last wave has been defeated
            else if (enemiesRemainingInWave <= 0)
            {
                AllWavesCleared();
            }
        }
    }

    private void AllWavesCleared()
    {
        //Mark this enemy manager as completed
        wavesCleared = true;
        remainingEnemyManagers -= 1;

        //Trigger the local events
        wavesClearedEvent?.Invoke();
        WavesCleared?.Invoke(this);

        //If all enemy managers have had all their waves completed, trigger
        if (remainingEnemyManagers <= 0)
            RoomCleared?.Invoke();

        //Set the flag in the flag manager stating that the enemies of this manager have been defeated
        FlagManager.instance.SetFlag(enemyManagerClearedEvent, true);
    }

    /// <summary>
    /// Triggers the spawning of a wave of enemies
    /// </summary>
    /// <param name="wave">The wave of enemies to trigger</param>
    /// <returns>Coroutine</returns>
    IEnumerator TriggerWave(EnemyWave wave)
    {
        //We check to see if we have an augment, and assign our respective augment method
        if (currentAugment != null)
        {
            augmentEnemy = currentAugment.augmentationEffect switch
            {
                EnemyAugmentation.AugmentationEffects.MovementSpeed => AugmentMovementSpeed,
                EnemyAugmentation.AugmentationEffects.Health => AugmentHealth,
                EnemyAugmentation.AugmentationEffects.AttackDamage => AugmentAttackDamage,
                _ => NoAugment,
            };
        }
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
        enemiesRemainingInWave += waveEnemyCount;

        //Calculate the delay there should be between spawning each enemy
        float enemySpawnDelay = wave.waveDuration / waveEnemyCount;

        //Spawn the enemies
        foreach (EnemyWaveGroup waveGroup in wave.enemies)
        {
            GameObject enemyPrefab = waveGroup.type;

            for (int i = 0; i < waveGroup.amount; i++)
            {
                Vector2 spawnPosition2D = UnityEngine.Random.insideUnitCircle * wave.spawnRadius;
                Vector3 spawnPosition = new Vector3(wave.spawnPosition.position.x + spawnPosition2D.x, wave.spawnPosition.position.y, wave.spawnPosition.position.z + spawnPosition2D.y);
                GameObject enemyGO = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
                enemyGO.GetComponent<BaseEnemyBehavior>().activated = true; //Activate the enemy
                augmentEnemy?.Invoke(enemyGO); //Augment the enemy
                enemiesSpawned.Add(enemyGO); //Save this enemy to the list of enemies spawned by this manager
                yield return new WaitForSeconds(enemySpawnDelay);

                //Spawn the spotlight for the enemy
                /*if (spotlightPrefab != null && enemyGO != null)
                {
                    GameObject spotlightGO = Instantiate(spotlightPrefab);
                    spotlightGO.GetComponent<SpotlightController>().target = enemyGO.transform;
                }*/
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
    /// Function that sets the current augment for the enemies in the room
    /// </summary>
    /// <param name="augment"></param>
    public void SetCurrentAugment(EnemyAugmentation augment)
    {
        if (augment != null)
        {
            currentAugment = augment;
        }
    }

    /// <summary>
    /// Function that takes in an enemy GameObject and augments its health
    /// </summary>
    /// <param name="enemyGO">GameObject of the enemy we are augmenting</param>
    /// <returns>Boolean stating whether or not it successfully applied the augment</returns>
    private bool AugmentHealth(GameObject enemyGO)
    {
        if (enemyGO.TryGetComponent(out BaseEnemyHealth enemyHealth))
        {
            float hp = enemyHealth.HP;
            float multiplier = (currentAugment.GetHealthIncrease() / 100) * hp;
            enemyHealth.HP = multiplier + hp;
            return true;
        }
        return false;
    }

    /// <summary>
    /// Function that takes in an enemy GameObject and augments its attack damage
    /// </summary>
    /// <param name="enemyGO">GameObject of the enemy we are augmenting</param>
    /// <returns>Boolean stating whether or not it successfully applied the augment</returns>
    private bool AugmentAttackDamage(GameObject enemyGO)
    {
        if (enemyGO.TryGetComponent(out BaseEnemyBehavior enemyBehavior))
        {
            float attackDamage = enemyBehavior.attackDamage;
            float multiplier = (currentAugment.GetAttackDamageIncrease() / 100) * attackDamage;
            enemyBehavior.attackDamage = multiplier + attackDamage;
            return true;
        }
        return false;
    }

    /// <summary>
    /// Function that takes in an enemy GameObject and augments its movement speed
    /// </summary>
    /// <param name="enemyGO">GameObject of the enemy we are augmenting</param>
    /// <returns>Boolean stating whether or not it successfully applied the augment</returns>
    private bool AugmentMovementSpeed(GameObject enemyGO)
    {
        if (enemyGO.TryGetComponent(out BaseEnemyBehavior enemyBehavior))
        {
            float navMeshSpeed = enemyBehavior.navMeshAgent.speed;
            float multiplier = (currentAugment.GetMovementSpeedIncrease() / 100) * navMeshSpeed;
            enemyBehavior.navMeshAgent.speed = multiplier + navMeshSpeed;
            return true;
        }
        return false;
    }

    /// <summary>
    /// Function that takes in an enemy GameObject and augments nothing
    /// </summary>
    /// <param name="enemyGO">GameObject of the enemy we are augmenting</param>
    /// <returns>Boolean stating whether or not it successfully applied the augment</returns>
    private bool NoAugment(GameObject enemyGO)
    {
        Debug.Log("No augment assigned to " + enemyGO.name);
        return false;
    }
    #endregion
}
