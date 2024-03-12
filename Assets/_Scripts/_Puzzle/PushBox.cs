using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class to identify pushbox
/// </summary>
public class PushBox : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] public bool isPushed;
    [SerializeField] private float pushForce;

    private void Start()
    {
        isPushed = false;
    }

    private void Update()
    {
        Vector3 curVelocity = rb.velocity;

        Vector3 direction = Vector3.Project(curVelocity, transform.right);
        direction = direction.normalized;
        Debug.Log(direction.magnitude);
        if (!isPushed)
        {
            rb.velocity = Vector3.zero;
        }
        else if (isPushed)
        {
            rb.velocity = direction.normalized * pushForce;
        }
    }


}
