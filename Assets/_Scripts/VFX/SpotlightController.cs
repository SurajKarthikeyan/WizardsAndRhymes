using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotlightController : MonoBehaviour
{
    public Transform target; // Reference to the target object (the object the spotlight will follow)
    public float yOffset = 2f; // Vertical offset from the target

    private void Update()
    {
        if (target != null)
        {
            // Get the target's position
            Vector3 targetPos = target.position;

            // Set the spotlight's position to be above the target with the yOffset
            Vector3 spotlightPos = new Vector3(targetPos.x, targetPos.y + yOffset, targetPos.z);

            // Set the spotlight's position
            transform.position = spotlightPos;

            // Look at the target
            transform.LookAt(targetPos);
        }
        else
        {
            Debug.LogWarning("Target reference is not set in the SpotlightController script.");
        }
    }
}