using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeslaTower : MonoBehaviour
{

    #region Vars
    [SerializeField] private List<GameObject> electricEffectList;
    #endregion

    #region CustomMethods

    public void DisableElectricEffect()
    {
        for (int i = 0; i < electricEffectList.Count; i++)
        {
            electricEffectList[i].SetActive(false);
        }
    }

    #endregion

}

