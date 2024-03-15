using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class UITransitionManager : MonoBehaviour
{
    public CinemachineVirtualCamera mainMenuCamera;
    public CinemachineVirtualCamera leftPanelCamera;
    public CinemachineVirtualCamera rightPanelCamera;

    private CinemachineVirtualCamera currentCamera;

    void Start()
    {
        // Set the initial camera
        currentCamera = mainMenuCamera;
        mainMenuCamera.Priority = 3; // Highest priority for main menu camera
        leftPanelCamera.Priority = 2;
        rightPanelCamera.Priority = 2;
    }

    void Update()
    {
        // Check mouse cursor position and update camera priority accordingly
        UpdateCameraPriority();
    }

    void UpdateCameraPriority()
    {
        Vector3 mousePosition = Input.mousePosition;

        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        // Check if the mouse is on the left side of the screen
        if (mousePosition.x < screenWidth / 3f)
        {
            SwitchToCamera(leftPanelCamera);
        }
        // Check if the mouse is on the right side of the screen
        else if (mousePosition.x > (2f * screenWidth) / 3f)
        {
            SwitchToCamera(rightPanelCamera);
        }
        // If not on the left or right, switch to main menu camera
        else
        {
            SwitchToCamera(mainMenuCamera);
        }
    }

    void SwitchToCamera(CinemachineVirtualCamera targetCamera)
    {
        if (currentCamera != targetCamera)
        {
            currentCamera.Priority = 1; // Lower priority for the current camera
            targetCamera.Priority = 3; // Higher priority for the target camera
            currentCamera = targetCamera;
        }
    }
}
