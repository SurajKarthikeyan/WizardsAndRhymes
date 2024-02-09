using UnityEngine;

/// <summary>
/// Enemy behavior class for enemy that melee attacks and then retreats afterwards
/// </summary>
public class MeleeRetreatEnemy : BaseEnemyBehavior
{
    #region Variables
    [Tooltip("Transform of the player to follow")]
    [SerializeField]
    private Transform player;
    #endregion


    #region Unity Methods
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
    #endregion

    #region Custom Methods
    #endregion
}
