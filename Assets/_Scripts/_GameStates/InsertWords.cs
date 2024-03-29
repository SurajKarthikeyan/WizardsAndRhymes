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
            wordCanvasManager.ToggleCanvas(true);
        }
    }
    
}
