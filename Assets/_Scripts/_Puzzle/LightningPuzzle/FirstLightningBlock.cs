using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class representing the first lightning block in the series
/// </summary>
public class FirstLightningBlock : MonoBehaviour
{
    #region Vars

    [SerializeField] private LightningBlockManager lbManager;
    [SerializeField] public bool isActive;
    #endregion


    #region CustomMethods


    /// <summary>
    /// Function to try and start lightning chain in the manager
    /// </summary>
    public void StartLightingChain()
    {

        if (isActive)
        {
            lbManager.CheckAllBlocks();
        }
    }

    
    /// <summary>
    /// Disables the first electro block if and only if the chaining is finished
    /// </summary>
    public void DisableFurtherChaining()
    {
        isActive = false;
    }

    #endregion
    
}
