using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Makes objects float up & down while gently spinning.
/// </summary>
public class Floater : MonoBehaviour
{
    #region Variables
    [Tooltip("The degrees per second for this object to rotate")]
    [SerializeField] public float degreesPerSecond = 15.0f;
    [Tooltip("The amplitude of the sine wave used to make this object float up and down")]
    [SerializeField] float amplitude = 0.5f;
    [Tooltip("The frequency  of the sine wave used to make this object float up and down")]
    [SerializeField] float frequency = 1f;
    [Tooltip("Whether or not to apply a random time offset to the sine wave used to make this object float up and down")]
    [SerializeField] bool randomizeStartingHeight;
    [Tooltip("Whether or not to start at a random rotation")]
    [SerializeField] bool randomizeStartingRotation;
    [Tooltip("Whether or not to use unscaled time")]
    [SerializeField] bool useUnscaledTime = true;

    [Tooltip("The starting position of this object")]
    Vector3 posOffset = new Vector3();
    [Tooltip("The offset to apply to the sine wave for this object")]
    float sineOffset = 0f;
    #endregion

    /// <summary>
    /// Store the starting position of the object and apply random offsets
    /// </summary>
    virtual protected void Start()
    {
        posOffset = transform.localPosition;

        //Randomize sine offset
        if (randomizeStartingHeight)
            sineOffset = Random.Range(0.0f, 2*Mathf.PI);

        //Randomize starting rotation
        if (randomizeStartingRotation)
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, Random.Range(0.0f, 359.0f), transform.eulerAngles.z);
    }

    /// <summary>
    /// Float and rotate
    /// </summary>
    virtual protected void Update()
    {
        float time = Time.time;
        float deltaTime = Time.deltaTime;
        if (useUnscaledTime)
        {
            time = Time.unscaledTime;
            deltaTime = Time.unscaledDeltaTime;
        }

        // Spin object around Y-Axis
        transform.Rotate(new Vector3(0f, deltaTime * degreesPerSecond, 0f), Space.World);

        // Float up/down with a Sin()
        Vector3 tempPos = transform.localPosition;
        tempPos.y = posOffset.y;
        tempPos.y += Mathf.Sin((time + sineOffset) * Mathf.PI * frequency) * amplitude;

        transform.localPosition = tempPos;
    }
}

// Floater v0.0.2
// by Donovan Keith
//
// [MIT License](https://opensource.org/licenses/MIT)
