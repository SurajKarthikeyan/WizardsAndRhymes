using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningBlock : MonoBehaviour
{
    #region Variables
    [SerializeField] public bool isOn;
    [SerializeField] private LightningBlock previousGO;
    [SerializeField] private bool isFirstBlock;

    #endregion


    #region UnityMethods

    private void Update()
    {
        if (!isFirstBlock)
        {
            if (previousGO.isOn)
            {
                isOn = true;
                Debug.Log(gameObject.name);
            }
        }
    }

    #endregion
    

    #region CustomMethods

    

    #endregion
}
