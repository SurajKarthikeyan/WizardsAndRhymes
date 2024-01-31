using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Enemy type that follows the player
/// </summary>
public class FollowPlayerEnemy : BaseEnemy
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
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    /// <summary>
    /// Unity method called every frame update
    /// </summary>
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (m_CurrentHP > 0)
        {
            navMeshAgent.destination = player.position;
        }
    }
    #endregion

    #region Custom Methods
    /// <summary>
    /// Helper method called upon each enemy death, allows for easy implementation
    /// of different enemy death behaviors
    /// </summary>
    protected override void EnemyDeathHelper()
    {
        Debug.Log("FollowPlayer Death Helper");
    }
    #endregion
    #endregion
}
