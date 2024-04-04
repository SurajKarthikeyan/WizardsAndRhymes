using UnityEngine;

/// <summary>
/// Enemy behavior class for enemy that melee attacks and then retreats afterwards
/// </summary>
public class MeleeRetreatEnemy : BaseRetreatEnemy
{
    #region Variables
    [Header("Melee specific attack variables")]
    [Tooltip("Distance in which the enemy will lunge at the player")]
    [SerializeField]
    private float meleeLungeDistance = 5f;

    [Tooltip("Force in which the enemy will lunge at the player with")]
    [SerializeField]
    private float meleeLungeForce = 10f;
    #endregion

    #region Unity Methods   
    /// <summary>
    /// Unity method called every frame update
    /// </summary>
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (activated && hasBeenSeen)
        {
            //Code handles looking at the player when not lunging at the player
            if (behaviorState != EnemyBehaviorState.MeleeAttacking)
            {
                LookAtPlayer();
            }
        }
    }
    #endregion

    #region Custom Methods
    /// <summary>
    /// Function that makes this enemy lunge at the player
    /// </summary>
    private void LungeAttack()
    {
        skellyAnimator.SetTrigger("meleeCharge");
        //Temporarily disable the navMesh because Unity physics do not like it
        navMeshAgent.enabled = false;
        rb.isKinematic = false;
        //Add a force in the direction the enemy is facing
        rb.AddForce((PlayerController.instance.transform.position - transform.position).normalized * meleeLungeForce, ForceMode.Impulse);
    }

    /// <summary>
    /// Evaluates the distance from the player to assist in determining the next state of behavior
    /// </summary>
    protected override void EvaluateDistanceFromPlayer()
    {
        float currDistance = Vector3.Distance(transform.position, PlayerController.instance.transform.position);
        //If we are retreating after the lunge attack
        if (behaviorState == EnemyBehaviorState.Retreating)
        {
            //If the navmesh has no current action, we make a full retreat
            if (!navMeshAgent.hasPath)
            {
                FullRetreat();
            }
            //Otherwise, we wait for it to finish its path
            else
            {
                if ((navMeshAgent.pathStatus == UnityEngine.AI.NavMeshPathStatus.PathComplete &&
                    currDistance > retreatDistance) || navMeshAgent.pathStatus == UnityEngine.AI.NavMeshPathStatus.PathPartial)
                {
                    StartCoroutine(PausePlayerTracking(1f));
                    navMeshAgent.speed = originalSpeed;
                }
            }
        }
        //This is when we are following the player at a great distance
        else if (currDistance > maxDistance && behaviorState != EnemyBehaviorState.MeleeAttacking)
        {
            behaviorState = EnemyBehaviorState.TrackingPlayer;
            navMeshAgent.destination = PlayerController.instance.transform.position;
        }
        //If the enemy is too close to the player
        else if (currDistance < minDistance && behaviorState != EnemyBehaviorState.MeleeAttacking)
        {
            SmallRetreat();
        }
        //If the enemy is in between the max and the min
        else
        {
            SpecializedBehavior();
        }
    }

    /// <summary>
    /// Special behavior for this enemy: a melee attack and then retreat
    /// </summary>
    protected override void SpecializedBehavior()
    {
        //If the enemy is tracking the player and in range, we begin to attack
        if (behaviorState == EnemyBehaviorState.TrackingPlayer)
        {
            behaviorState = EnemyBehaviorState.MeleeAttacking;
        }
        //If the enemy is ready to attack
        else if (behaviorState == EnemyBehaviorState.MeleeAttacking)
        {
            //If the enemy is far enough away to lunge
            if (Vector3.Distance(transform.position, PlayerController.instance.transform.position) >= meleeLungeDistance)
            {
                LungeAttack();
            }
            //If not, we back up to lunge
            else
            {
                navMeshAgent.updateRotation = false;
                SmallRetreat();
            }
        }
    }
    #endregion
}
