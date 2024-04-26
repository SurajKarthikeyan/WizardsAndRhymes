using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AYellowpaper.SerializedCollections;
using UnityEngine;
using UnityEngine.SceneManagement;

/**
 * Class that is responsible for all game logic and state flags
 */
public class FlagManager : Singleton<FlagManager>
{
    #region Variables
    [Tooltip("Dictionary containing all the flags throughout the game")]
    private static Dictionary<string, bool> flags = new Dictionary<string, bool>();

    public List<string> wordFlags = new List<string>();
    
    public SerializedDictionary<string, List<GameObject>> flagObjects;

    [Header("Gate Objects")]
    public Animator puzzle1Gate;

    public Animator puzzle2Gate;

    public GameObject spawnPoint1;
    
    public GameObject spawnPoint2;
    
    public GameObject spawnPoint3;

    public GameObject dialogueTrigger1;

    public GameObject dialogueTrigger2;

    public GameObject dialogueTrigger3;

    public GameObject rapRockTrigger;

    public static bool hasReloaded;
    
    #endregion
    
    #region Unity Methods

    
    private void Start()
    {
        
        if (GetFlag("puzzle2Completed"))
        {
            PlayerController.instance.gameObject.transform.position = spawnPoint3.transform.position;
        }
        else if (GetFlag("puzzle1Completed"))
        {
            PlayerController.instance.gameObject.transform.position = spawnPoint2.transform.position;
        }
        else
        {
            PlayerController.instance.gameObject.transform.position = spawnPoint1.transform.position;
        }

        foreach (string flag in flagObjects.Keys)
        {
            List<GameObject> flagObjectList = flagObjects[flag];
            foreach (GameObject flagObject in flagObjectList)
            {
                if (flagObject.TryGetComponent(out IFlagObject flagSetter))
                {
                    if (GetFlag(flag))
                    {
                        flagSetter.ObjectFlagSetState(true);
                    }

                    else
                    {
                        flagSetter.ObjectFlagSetState(false);
                    }
                }
            }
        }

        if (GetFlag("enemyWave1Completed"))
        {
            dialogueTrigger1.GetComponent<Collider>().enabled = false;
        }
        if (GetFlag("enemyWave2Completed"))
        {
            dialogueTrigger2.GetComponent<Collider>().enabled = false;
            puzzle1Gate.SetTrigger("interaction");
        }
        if (GetFlag("enemyWave3Completed"))
        {
            puzzle2Gate.SetTrigger("interaction");
        }
    }

    private void Update()
    {
        if (GetFlag("puzzle2Completed") && !dialogueTrigger3.GetComponent<DialogueTriggerer>().alreadyPlayed
            && !GetFlag("enemyWave3Completed"))
        {
            dialogueTrigger3.GetComponent<Collider>().enabled = true;
        }
        else if (GetFlag("puzzle1Completed") && !dialogueTrigger2.GetComponent<DialogueTriggerer>().alreadyPlayed
                 && !GetFlag("enemyWave2Completed"))
        {
            dialogueTrigger2.GetComponent<Collider>().enabled = true;
        }

        if (!rapRockTrigger.activeInHierarchy && HasAllWordFlags() && GetFlag(("enemyWave3Completed")))
        {
            rapRockTrigger.SetActive(true);
        }
    }
    #endregion

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
        foreach (var flagKey in flags.Keys)
        {
            Debug.Log("[" + flagKey + ", " + flags[flagKey] + "]");
        }
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
