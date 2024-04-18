using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainAreaInstance : MonoBehaviour
{
    #region Vars

    [Tooltip("Singleton Instance")]
    public static MainAreaInstance instance;

    #endregion

    #region UnityMethods

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogError("Multiple instances of MainAreaInstance" + gameObject.name);
        }
    }

    #endregion
}
