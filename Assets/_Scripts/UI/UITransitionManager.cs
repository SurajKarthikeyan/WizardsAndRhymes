using UnityEngine;
using Cinemachine;
using UnityEngine.UI;

public class UITransitionManager : MonoBehaviour
{
    public CinemachineVirtualCamera camera1;
    public CinemachineVirtualCamera camera2;
    public Button button1;
    public Button button2;
    public Button button3;
    public Button button4;

    void Start()
    {
        // Set the initial state
        camera1.Priority = 2; // Higher priority for camera1
        camera2.Priority = 1; // Lower priority for camera2
        button1.onClick.AddListener(OnClickButton1);
        button4.onClick.AddListener(OnClickButton4);

        // Initially, buttons 3-4 are inactive
        button3.gameObject.SetActive(false);
        button4.gameObject.SetActive(false);
    }

    void OnClickButton1()
    {
        // Switch to camera2
        camera1.Priority = 1; // Lower priority for camera1
        camera2.Priority = 2; // Higher priority for camera2

        // Activate buttons 2-4 and deactivate button1
        button1.gameObject.SetActive(false);
        button2.gameObject.SetActive(true);
        button3.gameObject.SetActive(true);
        button4.gameObject.SetActive(true);
    }

    void OnClickButton4()
    {
        // Switch back to camera1
        camera1.Priority = 2; // Higher priority for camera1
        camera2.Priority = 1; // Lower priority for camera2

        // Activate button1 and deactivate buttons 2-4
        button1.gameObject.SetActive(true);
        button2.gameObject.SetActive(false);
        button3.gameObject.SetActive(false);
        button4.gameObject.SetActive(false);
    }
}