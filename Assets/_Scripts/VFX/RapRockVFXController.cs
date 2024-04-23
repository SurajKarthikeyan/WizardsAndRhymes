using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

/// <summary>
/// Controls the VFX on the rap rock
/// </summary>
public class RapRockVFXController : MonoBehaviour
{
    [Tooltip("The VisualEffect component to enable")]
    [SerializeField] VisualEffect vfx;
    [Tooltip("The GameObjects to activate")]
    [SerializeField] GameObject[] gameObjectsToActivate;
    
    /// <summary>
    /// Enables the VFX on the rap rock
    /// </summary>
    public void EnableVFX()
    {
        vfx.Play();
        foreach (GameObject GO in gameObjectsToActivate)
            GO.SetActive(true);
    }
}
