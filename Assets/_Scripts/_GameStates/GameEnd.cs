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
    [Tooltip("This is a debug boolean used to test if the level is the 'final' level in the game(UnusedATM)")]
    [SerializeField] private bool isLastRoom;

    [Tooltip("This is here to ensure that once the room has been cleared it doesn't clear again")]
    [HideInInspector] private bool hasPassed;

    [Tooltip("Door to Instantiate")]
    [SerializeField] private GameObject doorRef;
    #endregion

    #region UnityMethods

    private void Start()
    {
        hasPassed = true;
    }

    private void Update()
    {
        if (EnemyManager.enemyManager.enemies.Count <= 0 && hasPassed)
        {
            doorRef.SetActive(true);
            hasPassed = false;
        }
    }

    #endregion
}
