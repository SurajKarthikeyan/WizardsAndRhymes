using UnityEngine;

/// <summary>
/// Enemy behavior class for enemy that melee attacks and then retreats afterwards
/// </summary>
public class MeleeRetreatEnemy : BaseEnemyBehavior
{
    #region Variables
    

    [Header("Retreating variables")]
    [SerializeField]
    private float retreatDistance = 20f;


    [Header("Melee specific attack variables")]
    [Tooltip("Distance in which the enemy will lunge at the player")]
    [SerializeField]
    private float meleeLungeDistance = 5f;

    [Tooltip("Force in which the enemy will lunge at the player with")]
    [SerializeField]
    private float meleeLungeForce = 10f;


    [Tooltip("Rigidbody of this enemy")]
    private Rigidbody rb;
    #endregion

    #region Unity Methods
    /// <summary>
    /// Method called on scene load
    /// </summary>
    protected override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody>();
    }
    /// <summary>
    /// Unity method called every frame update
    /// </summary>
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (activated)
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
                        if (navMeshAgent.pathStatus == UnityEngine.AI.NavMeshPathStatus.PathComplete ||
                            currDistance > retreatDistance)
                        {
                            behaviorState = EnemyBehaviorState.TrackingPlayer;
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
                    Retreat();
                }
                //If the enemy is in between the max and the min
                else
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
                            Retreat();
                        }
                    }
                }
            }
            //Code handles looking at the player when not lunging at the player
            if (behaviorState != EnemyBehaviorState.Retreating && 
                behaviorState != EnemyBehaviorState.MeleeAttacking)
            {
                Vector3 lookPos = PlayerController.instance.transform.position - transform.position;
                lookPos.y = 0;
                Quaternion rotation = Quaternion.LookRotation(lookPos);
                transform.rotation = rotation;
            }
        }
    }
    #endregion

    #region Custom Methods
    /// <summary>
    /// Function that lets this enemy move away from the player
    /// </summary>
    private void Retreat(float retreatMultiplier = 1)
    {
        //Calculates a vector pointing away from the player and moves the navMesh there
        Vector3 dirToPlayer = transform.position - PlayerController.instance.transform.position;
        Vector3 runPos = transform.position + dirToPlayer * retreatMultiplier;
        navMeshAgent.SetDestination(runPos);
    }

    /// <summary>
    /// Function that has this enemy attempt to get to a farther distance away
    /// </summary>
    private void FullRetreat() 
    {
        //Picks a random point 20 units away from the player to travel to
        Vector3 randomPoint = Random.onUnitSphere * retreatDistance;
        randomPoint.y = 0;
        randomPoint += PlayerController.instance.transform.position;
        navMeshAgent.SetDestination(randomPoint);
    }


    /// <summary>
    /// Function that makes this enemy lunge at the player
    /// </summary>
    private void LungeAttack()
    {
        //Temporarily disable the navMesh because Unity physics do not like it
        navMeshAgent.enabled = false;
        rb.isKinematic = false;
        //Add a force in the direction the enemy is facing
        rb.AddForce((PlayerController.instance.transform.position - transform.position).normalized * meleeLungeForce, ForceMode.Impulse);
    }
    #endregion
}
