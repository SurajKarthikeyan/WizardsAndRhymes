using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    public float existenceTimeThreshold = 5f;

    public int damage;

    private void Awake()
    {
        Destroy(gameObject, existenceTimeThreshold);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Enemy>() != null)
        {
            other.gameObject.GetComponent<Enemy>().hp -= damage;
        }
        Destroy(gameObject);
    }
}
