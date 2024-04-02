using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class responsible for displaying status about the player through UI elements
/// </summary>
public class PlayerStatusUI : MonoBehaviour
{
    public GameObject word1;
    
    public GameObject word2;

    public GameObject word3;

    // Update is called once per frame
    void Update()
    {
        if (!word1.activeInHierarchy)
        {
            word1.SetActive(FlagManager.instance.GetFlag("word1"));
        }
        
        if (!word2.activeInHierarchy)
        {
            word2.SetActive(FlagManager.instance.GetFlag("word2"));
        }
        
        if (!word3.activeInHierarchy)
        {
            word3.SetActive(FlagManager.instance.GetFlag("word3"));
        }
    }
}
