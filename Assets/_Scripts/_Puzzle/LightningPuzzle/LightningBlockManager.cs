using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LightningBlockManager : MonoBehaviour
{
    #region Vars

    [Tooltip("List of lighting blocks AND generator (PLACE THEM IN ORDER FROM FIRST TO LAST(GENERATOR IS LAST))")]
    [SerializeField] private List<GameObject> lightBlockList;
    [Tooltip("Reference to the generator of this lightining system")]
    [SerializeField] private Generator lightningGenerator;
    #endregion


    #region CustomMethods

    public void CheckAllBlocks()
    {
        if (lightBlockList.All(obj => obj.activeSelf))  // checks if all objects are enabled in the list
        {
            lightningGenerator.Hodor();
            
        }
    }

    #endregion
}
