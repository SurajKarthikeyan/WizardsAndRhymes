using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Tool for setting the time scale while the game is running
/// </summary>
public class TimeScale : MonoBehaviour
{
    [Tooltip("The time scale to set")]
    [SerializeField] float timeScale = 1;

    /// <summary>
    /// Called when value is changed. Updates the time scale
    /// </summary>
    private void OnValidate()
    {
        if (Application.isPlaying)
        {
            Time.timeScale = timeScale;
        }
    }
}
