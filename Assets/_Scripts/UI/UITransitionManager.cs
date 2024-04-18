using UnityEngine;
using Cinemachine;
using UnityEngine.UI;

public class UITransitionManager : MonoBehaviour
{
    public CinemachineVirtualCamera camera1;
    public CinemachineVirtualCamera camera2;
    public CinemachineVirtualCamera camera3;
    public Button button1;
    public Button button2;
    public Button button3;
    public Button button4;
    public Button button5;

    void Start()
    {
        // Set the initial state
        camera1.Priority = 3; // Highest priority for camera1
        camera2.Priority = 2; // Medium priority for camera2
        camera3.Priority = 1; // Lowest priority for camera3

        button1.onClick.AddListener(OnClickButton1);
        button2.onClick.AddListener(OnClickButton2);
        button3.onClick.AddListener(OnClickButton3);
        button4.onClick.AddListener(OnClickButton4);
        button5.onClick.AddListener(OnClickButton5);

        // Initially, buttons 3-4 are inactive
        button3.gameObject.SetActive(true); // Set button3 active initially
        button4.gameObject.SetActive(false);
        // button5.gameObject.SetActive(false); // Set button5 inactive initially
    }

    void OnClickButton1()
    {
        // Switch to camera2
        camera1.Priority = 2; // Medium priority for camera1
        camera2.Priority = 3; // Highest priority for camera2
        camera3.Priority = 1; // Lowest priority for camera3

        // Activate buttons 2-4 and deactivate button1
        // button1.gameObject.SetActive(false);
        button2.gameObject.SetActive(true);
        button3.gameObject.SetActive(true);
        button4.gameObject.SetActive(true);
    }

    void OnClickButton2()
    {
        // Switch to camera3
        camera1.Priority = 1; // Lowest priority for camera1
        camera2.Priority = 2; // Medium priority for camera2
        camera3.Priority = 3; // Highest priority for camera3

        // Activate buttons 1, 3-4 and deactivate button2
        // button1.gameObject.SetActive(true);
        button2.gameObject.SetActive(false);
        button3.gameObject.SetActive(true);
        button4.gameObject.SetActive(true);
    }

    void OnClickButton3()
    {
        // Switch to camera3
        camera1.Priority = 1; // Lowest priority for camera1
        camera2.Priority = 2; // Medium priority for camera2
        camera3.Priority = 3; // Highest priority for camera3
    }

    void OnClickButton4()
    {
        // Switch back to camera1
        camera1.Priority = 3; // Highest priority for camera1
        camera2.Priority = 2; // Medium priority for camera2
        camera3.Priority = 1; // Lowest priority for camera3
    }

    void OnClickButton5()
    {
        // Switch back to camera1
        camera1.Priority = 3; // Highest priority for camera1
        camera2.Priority = 2; // Medium priority for camera2
        camera3.Priority = 1; // Lowest priority for camera3
    }
}
