using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    public float existenceTimeThreshold = 5f;

    private void Awake()
    {
        Destroy(gameObject, existenceTimeThreshold);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
}
