using UnityEngine;

/// <summary>
/// Enemy type that follows the player and strafes around them before attacking
/// </summary>
public class MeleeStrafeEnemy : BaseStrafeEnemy
{
    #region Variables
    [Header("Melee specific attack variables")]
    [Tooltip("Distance in which the enemy will lunge at the player")]
    [SerializeField]
    private float meleeLungeDistance = 5f;

    [Tooltip("Force in which the enemy will lunge at the player with")]
    [SerializeField]
    private float meleeLungeForce = 10f;

    [Tooltip("Boolean determining whether or not we strafe right or not")]
    private bool strafeRight;

    [Tooltip("Current time elapsed in which this enemy is strafing")]
    private float strafeTime;

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
                    behaviorState = EnemyBehaviorState.TrackingPlayer;
                }
            }
            //Else statement is when the enemy is not lunging at the player
            else
            {
                float currDistance = Vector3.Distance(transform.position, PlayerController.instance.transform.position);
                //If the enemy is too far from the player, it will continue to follow it
                if (currDistance > maxDistance)
                {
                    behaviorState = EnemyBehaviorState.TrackingPlayer;
                    navMeshAgent.destination = PlayerController.instance.transform.position;
                }
                //If the enemy is too close to the player
                else if (currDistance < minDistance)
                {
                    SmallRetreat();
                }
                //If the enemy is in between the max and the min
                else
                {
                    //If the enemy is either coming into range from tracking the player or is already strafing
                    if (behaviorState == EnemyBehaviorState.TrackingPlayer || behaviorState == EnemyBehaviorState.Strafing)
                    {
                        /**
                         * We check to see if the enemy has strafed long enough, and if so, set its state to attacking
                         * and also reset the strafe timer
                         */
                        if (strafeTime >= strafeTimeThreshold)
                        {
                            behaviorState = EnemyBehaviorState.MeleeAttacking;
                            strafeTime = 0;
                        }
                        //This else runs when the enemy is still strafing or just about to start strafing
                        else
                        {
                            //If the enemy is about to start strafing, determine whether it will be right or left
                            if (behaviorState == EnemyBehaviorState.TrackingPlayer)
                            {
                                int strafeRightNum = Random.Range(0, 1);
                                strafeRight = strafeRightNum != 0;
                                behaviorState = EnemyBehaviorState.Strafing;
                            }
                            //We then begin to strafe and calculate how long we have been strafing
                            Strafe(strafeRight);
                            strafeTime += Time.deltaTime;
                        }
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
        //Temporarily disable the navMesh because Unity physics do not like it
        navMeshAgent.enabled = false;
        rb.isKinematic = false;
        //Add a force in the direction the enemy is facing
        rb.AddForce((PlayerController.instance.transform.position - transform.position).normalized * meleeLungeForce, ForceMode.Impulse);
    }
    #endregion
}
