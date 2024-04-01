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
    
    /// <summary>
    /// Function that is called whenever a collider enters this trigger
    /// </summary>
    /// <param name="other">Other collider that has entered this trigger</param>
    private void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.CompareTag("Player"))
        {
            is2D = true;
            camAnimator.SetBool("SwitchCam", is2D);
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
            camAnimator.SetBool("SwitchCam", is2D);
            PlayerController.instance.gridBasedControl = is2D;
        }
    }
}
