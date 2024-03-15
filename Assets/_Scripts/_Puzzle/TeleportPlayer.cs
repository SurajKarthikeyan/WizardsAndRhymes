using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportPlayer : MonoBehaviour
{

    [Tooltip("Transform of Destination Object")]
    [SerializeField] private Transform destinationTransform;

    #region UnityMethods

    private void OnTriggerEnter(Collider other)
    {
        PlayerController.instance.gameObject.transform.position = destinationTransform.position;
    }

    #endregion
    
}
