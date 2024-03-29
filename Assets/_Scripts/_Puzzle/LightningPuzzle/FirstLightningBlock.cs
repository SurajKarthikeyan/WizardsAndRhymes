using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstLightningBlock : MonoBehaviour
{
    #region Vars

    [SerializeField] private LightningBlockManager lbManager;

    #endregion


    #region CustomMethods


    /// <summary>
    /// Function to try and start lightning chain in the manager
    /// </summary>
    public void StartLightingChain()
    {
        lbManager.CheckAllBlocks();
    }

    #endregion
    
}
