using UnityEngine;

/// <summary>
/// Base class for a projectile
/// </summary>
public class Projectile : MonoBehaviour
{
    #region Variables
    [Tooltip("Time in seconds that this projectile exists before being destroyed")]
    [SerializeField]
    private float existenceTimeThreshold = 5f;

    [Tooltip("Instance of the damage type enum")]
    [SerializeField] private Health.DamageType dType;

    [Tooltip("Damage that this projectile deals")]
    public int damage;
    #endregion

    #region Unity Methods
    /// <summary>
    /// Function that calls immediately upon this scene start
    /// </summary>
    private void Awake()
    {
        Destroy(gameObject, existenceTimeThreshold);
    }

    /// <summary>
    /// Function that is called when this object's trigger collides with another collider
    /// </summary>
    /// <param name="other">Other collider that is contacted by this trigger</param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Health>(out var character))
        {
            character.TakeDamage(damage, dType);
        }
        Destroy(gameObject);
    }
    #endregion
}
