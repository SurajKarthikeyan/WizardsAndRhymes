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

    [SerializeField] private Color onColor;
    [SerializeField] private Color offColor;

    public GameObject platform;

    #endregion

    #region UnityMethods

    private void Start()
    {
        SetColor(isOn);
    }


    #endregion

    #region CustomMethods

    public void FlipLight()
    {
        isOn = !isOn;
        SetColor(isOn);
    }

    private void SetColor(bool lightOn)
    {
        if (lightOn)
        {
            lightReference.color = onColor;
        }

        else
        {
            lightReference.color = offColor;
        }
    }

    public void LightTrigger()
    {
        SetColor(true);
        platform.SetActive(false);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Projectile>() != null)
        {
            LightTrigger();
        }
    }
    #endregion
}
