using AYellowpaper.SerializedCollections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class used for physical attacks
/// </summary>
public class MeleeCollider : MonoBehaviour
{
    #region Variables
    [Tooltip("Reference to the player controller")]
    PlayerController player;

    [Tooltip("Base enemy class that is looked for when doing damage")]
    BaseEnemyBehavior enemy;
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
        if (other.gameObject.TryGetComponent<BaseEnemyBehavior>(out var enemy))
        {
            enemy.gameObject.GetComponent<Health>().TakeDamage(player.meleeDamage, Health.DamageType.Fire);
        }
    }
    #endregion
}
