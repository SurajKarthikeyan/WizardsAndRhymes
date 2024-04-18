using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using CodeMonkey.Utils;
using UnityEngine.Diagnostics;

public class ObjectivePointer : MonoBehaviour
{
    #region Vars

    public static ObjectivePointer instance;

    [Tooltip("Current target position; update as needed via script and the singleton")] 
    [SerializeField] public GameObject targetGO;
    #endregion

    #region UnityMethods

    private void Awake()
    {
        // Set singleton instance
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogError("Multiple instances of Objective Pointer" + this.gameObject);
        }
    }

    private void Update()
    {
        Vector3 direction = (targetGO.transform.position - this.transform.position).normalized;
        direction.y = 0f;
        transform.rotation = Quaternion.LookRotation(direction);
    }

    #endregion

    #region CustomMethods



    #endregion
}
