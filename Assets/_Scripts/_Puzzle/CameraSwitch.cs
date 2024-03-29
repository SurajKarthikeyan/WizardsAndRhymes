using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Class that handles switching between two Cinemachine cameras
/// </summary>
public class CameraSwitch : MonoBehaviour
{

    public Animator camAnimator;

    public static bool is2D;
    
    private void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.CompareTag("Player"))
        {
            is2D = true;
            camAnimator.SetBool("SwitchCam", is2D);
            PlayerController.instance.gridBasedControl = is2D;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            is2D = false;
            camAnimator.SetBool("SwitchCam", is2D);
            PlayerController.instance.gridBasedControl = is2D;
        }
    }
}
