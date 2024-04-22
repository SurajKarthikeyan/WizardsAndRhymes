using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnTeslaTowerDisable : MonoBehaviour
{
    #region Vars

    [SerializeField] private AK.Wwise.Event teslaTowerDisabled;
    #endregion

    #region UnityMethods

    public void DisableTeslaTower()
    {
        teslaTowerDisabled.Post(PlayerController.instance.gameObject);
    }

    #endregion
}
