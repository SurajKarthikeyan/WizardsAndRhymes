using UnityEngine;

/// <summary>
/// Enemy type that follows the player and strafes around them before attacking
/// </summary>
public class MeleeStrafeEnemy : BaseEnemyBehavior
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
