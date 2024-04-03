using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    #region Vars
    [Tooltip("Pressure Plate SFX")]
    [SerializeField] private AK.Wwise.Event pressurePlateSoundEffect;
    #endregion

    #region UnityMethods

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PushBox") || other.CompareTag("Player"))
        {
            //Player or pushbox implementation
        }
        else if (other.CompareTag("PushBoxEletric"))
        {
            //PushBoxEletric implementation
        }
    }

    #endregion
}
