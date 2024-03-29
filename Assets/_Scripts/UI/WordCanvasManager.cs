using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordCanvasManager : MonoBehaviour
{
    public bool canvasActivated;

    public Canvas wordCanvas;
    
    // // Start is called before the first frame update
    // void Start()
    // {
    //     
    // }
    //
    // // Update is called once per frame
    // void Update()
    // {
    //     
    // }


    public void ToggleCanvas(bool canvasOn)
    {

        canvasActivated = canvasOn;
        wordCanvas.gameObject.SetActive(canvasActivated);
        
        if (canvasActivated)
        {
            PlayerController.instance.DisablePlayerControls();
        }
        else
        {
            PlayerController.instance.EnablePlayerControls();
        }
    }
}
