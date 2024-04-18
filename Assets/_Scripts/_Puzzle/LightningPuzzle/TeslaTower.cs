using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Tesla tower script, disables when pressure plate says so
/// </summary>
public class TeslaTower : MonoBehaviour
{

    #region Vars
    [SerializeField] private List<GameObject> electricEffectList;
    #endregion

    #region CustomMethods

    /// <summary>
    /// Disables electric effects
    /// </summary>
    public void DisableElectricEffect()
    {
        for (int i = 0; i < electricEffectList.Count; i++)
        {
            electricEffectList[i].SetActive(false);
        }
    }

    #endregion

}

