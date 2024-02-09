using UnityEngine;

/// <summary>
/// Class used for physical attacks
/// </summary>
public class MeleeCollider : MonoBehaviour
{
    #region Variables
    [Tooltip("Reference to the player controller")]
    PlayerController player;
    #endregion

    #region Unity Methods
    /// <summary>
    /// Function that is called when the scene starts
    /// </summary>
    private void Start()
    {
        player = transform.GetComponentInParent<PlayerController>();
    }

    /// <summary>
    /// Function that is called when this trigger enters contact with another collider
    /// </summary>
    /// <param name="other">Other collider that is in contact with this trigger</param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out BaseEnemyHealth enemyHealth))
        {
            enemyHealth.TakeDamage(player.meleeDamage, Health.DamageType.Fire);
        }
    }
    #endregion
}
