using System.Collections;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Class used for physical attacks
/// </summary>
public class MeleeCollider : MonoBehaviour
{
    #region Variables
    [Tooltip("Reference to the player controller")]
    PlayerController player;

    [SerializeField] public Health.DamageType damageType;
    #endregion

    #region Unity Methods
    /// <summary>
    /// Function that is called when the scene starts
    /// </summary>
    private void Start()
    {
        player = transform.GetComponentInParent<PlayerController>();
        damageType = Health.DamageType.None;
    }

    /// <summary>
    /// Function that is called when this trigger enters contact with another collider
    /// </summary>
    /// <param name="other">Other collider that is in contact with this trigger</param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out BaseEnemyHealth enemyHealth))
        {
            if (enemyHealth.vulnerable)
            {
                enemyHealth.TakeDamage(player.meleeDamage, damageType);
                if (enemyHealth.transform.gameObject.TryGetComponent(out BaseEnemyBehavior enemy))
                {
                    enemy.Knockback(player.transform.forward);
                }
                
            }
        }
    }
    #endregion

}
