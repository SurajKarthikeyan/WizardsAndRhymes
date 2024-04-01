using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Defines a trigger used to start a crowd surf
/// </summary>
public class CrowdSurfTrigger : MonoBehaviour
{
    #region Variables
    [Tooltip("The crowd surf path triggered by this component")]
    [HideInInspector] public CrowdSurfPath crowdSurfPath;
    [Tooltip("Whether this trigger is at the end of the path instead of the start")]
    [HideInInspector] public bool end;

    [Tooltip("Whether the player is already in the trigger and should be ignored")]
    bool ignorePlayerInCollider;
    #endregion

    #region Unity Methods
    /// <summary>
    /// Start a crowd surf when the player enters this trigger
    /// </summary>
    /// <param name="other">The collider that entered the trigger</param>
    private void OnTriggerEnter(Collider other)
    {
        if (crowdSurfPath.PlayerOnPath || ignorePlayerInCollider) //Don't try to start a crowd-surf if the player is already crowd surfing
        {
            ignorePlayerInCollider = true;
            return;
        }

        if (other.CompareTag(PlayerController.PlayerTag))
        {
            crowdSurfPath.StartCrowdSurf(end);
        }
    }

    /// <summary>
    /// Reset values when the player exits this trigger
    /// </summary>
    /// <param name="other">The collider that exited this trigger</param>
    private void OnTriggerExit(Collider other)
    {
        ignorePlayerInCollider = false;
    }
    #endregion
}
