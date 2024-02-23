using UnityEngine;

/// <summary>
/// Base class for an enemy type that retreats from the player
/// </summary>
public class BaseRetreatEnemy : BaseEnemyBehavior
{
    #region Variables
    [Header("Retreating variables")]
    [Tooltip("Distance this enemy will retreat")]
    [SerializeField]
    protected float retreatDistance = 5f;

    [Tooltip("Speed this this enemy will retreat")]
    [SerializeField]
    protected float retreatSpeed = 5f;

    [Tooltip("Original speed of the enemy")]
    protected float originalSpeed;
    #endregion

    /// <summary>
    /// Method called on scene start
    /// </summary>
    protected override void Start()
    {
        base.Start();
        originalSpeed = navMeshAgent.speed;
        if (retreatSpeed <= 0)
        {
            retreatSpeed = originalSpeed;
        }
    }

    /// <summary>
    /// Unity method called every frame
    /// </summary>
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (activated && hasBeenSeen)
        {
            EvaluateBehavior();
            LookAtPlayer();
        }
    }

    /// <summary>
    /// Override of the base enemy's virtual function that evaluates this enemy's behavior
    /// and sets its next state
    /// </summary>
    protected override void EvaluateBehavior()
    {
        if (behaviorState != EnemyBehaviorState.Ice)
        {
            if (!rb.isKinematic)
            {
                /**
                 * Check is here because navMesh and Unity physics do not play nice with each other
                 * This is when the enemy is currently lunging at the player
                 */
                if (rb.velocity.magnitude <= 0.5f)
                {
                    //All this is to get the navMesh back following the player
                    navMeshAgent.enabled = true;
                    rb.isKinematic = true;
                    behaviorState = EnemyBehaviorState.Retreating;
                }
            }
            //Else statement is when the enemy is not lunging at the player
            else
            {
                //Next part of the enemy's logic is to evaluate its distance from the player
                EvaluateDistanceFromPlayer();
            }
        }

        else
        {
            navMeshAgent.speed = 0;
        }
    }
    /// <summary>
    /// Function that has this enemy attempt to get to a farther distance away
    /// </summary>
    protected void FullRetreat()
    {
        navMeshAgent.speed = retreatSpeed;
        //Picks a random point retreat distance units away from the player to travel to
        Vector3 randomPoint = Random.onUnitSphere * retreatDistance;
        randomPoint.y = 0;
        randomPoint += PlayerController.instance.transform.position;
        navMeshAgent.SetDestination(randomPoint);
    }
}
