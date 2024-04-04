using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/**
 * Class that is responsible for all game logic and state flags
 */
public class FlagManager : Singleton<FlagManager>
{
    #region Variables
    [Tooltip("Dictionary containing all the flags throughout the game")]
    private static Dictionary<string, bool> flags = new Dictionary<string, bool>();

    public List<string> wordFlags = new List<string>();
    
    [Header("Gate Objects")]

    public GameObject puzzle1Gate1;

    public GameObject puzzle1Gate2;
    
    public GameObject puzzle2Gate1;

    public GameObject puzzle2Gate2;

    public GameObject spawnPoint1;
    
    public GameObject spawnPoint2;
    
    public GameObject spawnPoint3;

    public GameObject dialogueTrigger1;

    public GameObject dialogueTrigger2;

    public GameObject dialogueTrigger3;

    public GameObject rapRockTrigger;

    public GameObject pauseMenu;
    
    #endregion
    
    #region Unity Methods

    
    private void Start()
    {
        dialogueTrigger1.SetActive(false);
        if (GetFlag("enemyWave3Completed") && HasAllWordFlags())
        {
            puzzle1Gate1.SetActive(true);
            puzzle1Gate2.SetActive(true);
            puzzle2Gate1.SetActive(true);
            puzzle2Gate2.SetActive(true);
        }
        else if (GetFlag("enemyWave2Completed"))
        {
            puzzle2Gate1.SetActive(false);
            puzzle2Gate2.SetActive(false);
        }
        
        else if (GetFlag("enemyWave1Completed"))
        {
            puzzle1Gate1.SetActive(false);
            puzzle1Gate2.SetActive(false);
        }

        if (GetFlag("puzzle2Completed"))
        {
            PlayerController.instance.gameObject.transform.position = spawnPoint3.transform.position;
            puzzle2Gate1.SetActive(false);
            puzzle2Gate2.SetActive(true);
            dialogueTrigger3.SetActive(true);
            dialogueTrigger2.SetActive(false);
        }
        else if (GetFlag("puzzle1Completed"))
        {
            PlayerController.instance.gameObject.transform.position = spawnPoint2.transform.position;
            puzzle1Gate1.SetActive(false);
            puzzle1Gate2.SetActive(true);
            dialogueTrigger2.SetActive(true);
        }
        else
        {
            dialogueTrigger1.SetActive(true);
            PlayerController.instance.gameObject.transform.position = spawnPoint1.transform.position;
        }
    }

    #endregion

    private void Update()
    {
        if (GetFlag("puzzle2Completed") && !dialogueTrigger3.activeInHierarchy && 
            !dialogueTrigger3.GetComponent<DialogueTriggerer>().alreadyPlayed)
        {
            dialogueTrigger3.SetActive(true);
        }
        else if (GetFlag("puzzle1Completed") && !dialogueTrigger2.activeInHierarchy && 
                 !dialogueTrigger2.GetComponent<DialogueTriggerer>().alreadyPlayed)
        {
            dialogueTrigger2.SetActive(true);
        }

        if (!rapRockTrigger.activeInHierarchy && HasAllWordFlags() && GetFlag(("enemyWave3Completed")))
        {
            rapRockTrigger.SetActive(true);
        }
    }

    #region Custom Methods
    
    /// <summary>
    /// Function that returns a flag based on a provided key if it exists, otherwise returns false;
    /// </summary>
    /// <param name="key">Key of the flag we wish to return</param>
    /// <returns>Flag of the given key, or false if key does not exist</returns>
    public bool GetFlag(string key)
    {
        if (flags.TryGetValue(key, out var flag))
        {
            return flag;
        }

        return false;
    }

    /// <summary>
    /// Function that sets a flag given a value
    /// </summary>
    /// <param name="key">Key of the flag we wish to set</param>
    /// <param name="value">Flag value we wish to set</param>
    public void SetFlag(string key, bool value)
    {
        flags[key] = value;
        Debug.Log("Just set flag " + key + " to be " + value);
    }

    /// <summary>
    /// Function that clears all the flags, used primarily upon scene load
    /// </summary>
    public void ClearFlags()
    {
        flags.Clear();
    }

    public bool HasAllWordFlags()
    {
        foreach (string wordFlag in wordFlags)
        {
            if (!(flags.ContainsKey(wordFlag) && flags[wordFlag]))
            {
                return false;
            }
        }

        return true;
    }
    
    #endregion
}
