using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Destroys this GameObject after a set lifetime
/// </summary>
public class DestroyAfterDelay : MonoBehaviour
{
    [Tooltip("How long to wait before destroying this GameObject")]
    [SerializeField] float lifetime;

    /// <summary>
    /// Invoke the function to destroy this GameObject once it's lifetime has elapsed
    /// </summary>
    private void Start()
    {
        Invoke(nameof(DestroyGameObject), lifetime);
    }

    /// <summary>
    /// Destroy this GameObject
    /// </summary>
    void DestroyGameObject()
    {
        Destroy(gameObject);
    }
}
