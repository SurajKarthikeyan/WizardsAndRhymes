using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for a projectile
/// </summary>
public class Projectile : MonoBehaviour
{
    /// <summary>
    /// Time in seconds that this projectile exists before being destroyed
    /// </summary>
    [SerializeField]
    private float existenceTimeThreshold = 5f;

    public int damage;

    private void Awake()
    {
        Destroy(gameObject, existenceTimeThreshold);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<BaseEnemy>() != null)
        {
            other.gameObject.GetComponent<Character>().HP -= damage;
        }
        Destroy(gameObject);
    }
}
