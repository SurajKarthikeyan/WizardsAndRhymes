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
        if (other.gameObject.GetComponent<BaseEnemy>() != null)
        {
            other.gameObject.GetComponent<BaseEnemy>().hp -= damage;
        }
        Destroy(gameObject);
    }
}
