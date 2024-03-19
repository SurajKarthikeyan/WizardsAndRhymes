using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that handles switching between two Cinemachine cameras
/// </summary>
public class CameraSwitch : MonoBehaviour
{

    public Animator camAnimator;

    public bool switchCam;
    // Start is called before the first frame update
    void Start()
    {
        switchCam = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.name);
        
    }
    private void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("AAHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHH");
            switchCam = !switchCam;
            camAnimator.SetBool("SwitchCam", switchCam);

        }
    }
}
