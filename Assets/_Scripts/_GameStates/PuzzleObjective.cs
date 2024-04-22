using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Deactivates the pointer objective when entering the puzzle
/// </summary>
public class PuzzleObjective : MonoBehaviour
{

    [SerializeField] private GameObject target;
    #region UnityMethods

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ObjectivePointer.instance.targetGO = target;
            Destroy(this.gameObject);
        }
    }

    #endregion
}
