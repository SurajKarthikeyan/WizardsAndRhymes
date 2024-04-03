using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsertWords : MonoBehaviour
{
    public WordCanvasManager wordCanvasManager;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerController.instance.canInteract = true;
            PlayerController.instance.interactable = wordCanvasManager;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && PlayerController.instance.interactable == null)
        {
            PlayerController.instance.canInteract = true;
            PlayerController.instance.interactable = wordCanvasManager;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerController.instance.canInteract = false;
            PlayerController.instance.interactable = null;
        }
    }
}
