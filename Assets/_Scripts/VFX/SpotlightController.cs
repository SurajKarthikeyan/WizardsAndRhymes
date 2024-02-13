using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotlightController : MonoBehaviour
{
    public Transform target; // Reference to the target object (the object the spotlight will shine on)

    private void Update()
    {
        if (target != null)
        {
            // Calculate the direction from the spotlight to the target
            Vector3 directionToTarget = target.position - transform.position;

            // Ensure the spotlight remains fixed to the ceiling (ceiling is usually along the y-axis)
            Vector3 fixedDirection = Vector3.ProjectOnPlane(directionToTarget, Vector3.up).normalized;

            // Calculate the rotation to point towards the target
            Quaternion targetRotation = Quaternion.LookRotation(fixedDirection, Vector3.up);

            // Apply the rotation to the spotlight
            transform.rotation = targetRotation;
        }
        else
        {
            Debug.LogWarning("Target reference is not set in the SpotlightController script.");
        }
    }
}