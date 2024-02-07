using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Enemy type that follows the player
/// </summary>
public class MeleeStrafeEnemy : BaseEnemyBehavior
{
    #region Class Variables
    #region AI Variables
    /// <summary>
    /// NavMeshAgent component to control behavior of enemy
    /// </summary>
    NavMeshAgent navMeshAgent;

    /// <summary>
    /// Transform of the player to follow
    /// </summary>
    public Transform player;
    #endregion
    #endregion

    #region Methods
    #region Unity Methods
    /// <summary>
    /// Unity method called before the first frame update
    /// </summary>
    protected override void Start()
    {
        base.Start();
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    /// <summary>
    /// Unity method called every frame update
    /// </summary>
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (activated)
        {
            navMeshAgent.destination = player.position;
        }
    }

    /// <summary>
    /// Unity method called whenever this object collides with another
    /// </summary>
    /// <param name="collision">Collision object that this enemy collides with</param>
    private void OnCollisionEnter(Collision collision)
    {
        if (activated)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                collision.gameObject.GetComponent<Health>().TakeDamage(attackDamage, Health.DamageType.Fire);
            }
        }
    }
    #endregion

    #region Custom Methods
    /// <summary>
    /// Helper method called upon each enemy death, allows for easy implementation
    /// of different enemy death behaviors
    /// </summary>
    protected override void EnemyDeath()
    {
        Debug.Log("FollowPlayer Death Helper");
    }
    #endregion
    #endregion
}
