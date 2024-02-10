using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that handles ending the current game / level
/// </summary>
public class GameEnd : MonoBehaviour
{
    #region Variable
    [Tooltip("This is a debug boolean used to test if the level is the 'final' level in the game")]
    [SerializeField] private bool isLastRoom;
    #endregion

    #region UnityMethods

    private void Update()
    {
        Debug.Log(EnemyManager.enemyManager.enemies.Count);
        if (EnemyManager.enemyManager.enemies.Count <= 0)
        {
            if (isLastRoom)
            {
                Debug.Log("FINAL LEVEL END");
            }
            else
            {
                Debug.Log("LEVEL END");
            }
        }
    }

    #endregion
}
