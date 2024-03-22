using System.Collections;
using System.Collections.Generic;
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
            is2D = !is2D;
            camAnimator.SetBool("SwitchCam", is2D);
            PlayerController.instance.topDownControl = is2D;
        }
    }
}
