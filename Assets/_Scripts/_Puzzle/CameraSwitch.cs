using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that handles switching between two Cinemachine cameras
/// </summary>
public class CameraSwitch : MonoBehaviour
{
    [Tooltip("Animator that moves the camera")]
    [SerializeField]
    private Animator camAnimator;

    [Tooltip("Boolean that states if the camera is in a top down state or not")]
    private static bool is2D;

    public GameObject angledCam;

    public Transform angledCamTransform;

    public bool isTransitioning;


    /// <summary>
    /// Function that is called whenever a collider enters this trigger
    /// </summary>
    /// <param name="other">Other collider that has entered this trigger</param>
    private void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.CompareTag("Player"))
        {
            is2D = true;
            angledCam.transform.position = angledCamTransform.position;
            angledCam.transform.rotation = angledCamTransform.rotation;
            if (!isTransitioning)
            {
                StartCoroutine(StopPlayerControl(0.8f));
                camAnimator.SetBool("SwitchCam", is2D);
            }
            PlayerController.instance.gridBasedControl = is2D;
        }
    }

    /// <summary>
    /// Function that is called whenever a collider exits this trigger
    /// </summary>
    /// <param name="other">Other collider that has exited this trigger</param>
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            is2D = false;
            if (!isTransitioning)
            {
                camAnimator.SetBool("SwitchCam", is2D);
                StartCoroutine(StopPlayerControl(0.8f));
            }
            PlayerController.instance.gridBasedControl = is2D;
        }
    }

    IEnumerator StopPlayerControl(float seconds)
    {
        isTransitioning = true;
        PlayerController.instance.DisablePlayerControls();
        yield return new WaitForSeconds(seconds);
        isTransitioning = false;
        PlayerController.instance.EnablePlayerControls();
    }
}
