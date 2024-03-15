using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Class to determine which face of the box is being pushed(you need with with PushBox)(Currently deprecated)
/// </summary>
public class BoxFace : MonoBehaviour
{
    #region Variables
    [Tooltip("Reference to parent PushBox script")]
    [SerializeField] private PushBox parentPushBox;
    [Tooltip("Sound Event for box push")]
    [SerializeField] private AK.Wwise.Event pushEvent;
    
    #endregion

    #region CustomMethods
    
    /// <summary>
    /// On Trigger enter checks if its the player then moves the box in direction
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            pushEvent.Post(this.gameObject);
            parentPushBox.isPushed = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            parentPushBox.isPushed = false;
        }
    }

    #endregion
}
