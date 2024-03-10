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
    [Tooltip("If this is the face the player pushes to move it 'forward'")]
    [SerializeField] private bool forward;

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
            //parentPushBox.Push(forward);
        }
    }

    /// <summary>
    /// On Trigger Stay checks if its the player then KEEPS moves the box in direction
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //parentPushBox.Push(forward);
        }
    }

    #endregion
}
