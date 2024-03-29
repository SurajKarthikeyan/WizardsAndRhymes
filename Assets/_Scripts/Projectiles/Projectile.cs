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
    [SerializeField] public Health.DamageType dType;

    [Tooltip("Damage that this projectile deals")]
    [SerializeField]
    private int damage;
    #endregion

    #region Unity Methods
    /// <summary>
    /// Function that calls immediately upon this scene start
    /// </summary>
    private void Awake()
    {
        Destroy(gameObject, existenceTimeThreshold);
        dType = Health.DamageType.None;
    }

    /// <summary>
    /// Function that is called when this object's trigger collides with another collider
    /// </summary>
    /// <param name="other">Other collider that is contacted by this trigger</param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Health>(out var character))
        {
            if (character.vulnerable)
            {
                character.TakeDamage(damage, dType);
                if (character.gameObject.TryGetComponent(out BaseEnemyBehavior enemy))
                {
                    enemy.Knockback(transform.forward);
                }
            }
        }

        else if (other.TryGetComponent<FirstLightningBlock>(out var lightningBlock))
        {
            lightningBlock.StartLightingChain();
        }
        Destroy(gameObject);
    }
    #endregion
}
