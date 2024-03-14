using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLight : MonoBehaviour
{
    #region Variables

    [Tooltip("Bool if the light is on or off, set here to set color")]
    public bool isOn;
    
    [Tooltip("Reference to the light on this gameobject")]
    [SerializeField] private Light lightReference;


    #region UnityMethods

    private void Start()
    {
        lightReference.enabled = isOn;
    }

    #endregion
    
    #region CustomMethods

    public void FlipLight()
    {
        isOn = !isOn;
        lightReference.enabled = isOn;
    }

    #endregion
    #endregion
}
