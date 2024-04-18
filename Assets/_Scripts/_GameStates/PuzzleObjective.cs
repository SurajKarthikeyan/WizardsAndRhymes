using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleObjective : MonoBehaviour
{

    #region UnityMethods

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ObjectivePointer.instance.gameObject.SetActive(false);
            Destroy(this.gameObject);
        }
    }

    #endregion
}
