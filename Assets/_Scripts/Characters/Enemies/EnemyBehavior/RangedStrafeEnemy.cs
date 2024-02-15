using System.Collections;
using UnityEngine;

/// <summary>
/// Enemy that will shoot a projectile when in range and strafe before shooting another
/// </summary>
public class RangedStrafeEnemy : BaseEnemyBehavior
{
    #region Variables
    [Tooltip("Maximum distance this enemy will be from the player before it shoots")]
    [SerializeField]
    private float maxDistance = 10f;

    [Tooltip("Minimum distance this enemy will be from the player before it shoots")]
    [SerializeField]
    private float minDistance = 1f;

    [Tooltip("Projectile object that this enemy shoots")]
    [SerializeField]
    private GameObject enemyProjectile;

    [Tooltip("Transform that this enemy shoots the projectile from")]
    [SerializeField]
    private Transform projectileSpawnPoint;

    [Tooltip("Cooldown in between every projectile shot by this enemy")]
    [SerializeField]
    private float shootCooldown = 1f;

    [Header("Strafing variables")]
    [Tooltip("Speed that this enemy strafes, if set to 0 it will take the speed of the navMesh agent")]
    [SerializeField]
    private float strafeSpeed;

    [Tooltip("Time in seconds in which this enemy will strafe for")]
    [SerializeField]
    private float strafeTimeThreshold;

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
        //Sets the default strafing speed to be the speed of the navMesh
        if (strafeSpeed <= 0)
        {
            strafeSpeed = navMeshAgent.speed;
        }
    }

    /// <summary>
    /// Unity method called every frame update
    /// </summary>
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
                    Retreat();
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
                            behaviorState = EnemyBehaviorState.RangedAttacking;
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
                    else if (behaviorState == EnemyBehaviorState.RangedAttacking)
                    {
                        Debug.Log("Shooting Projectile");
                        ShootProjectile();
                    }
                }
            }
            //Rest of the code calculates rotation that the enemy is looking in and smoothly rotates it
            Vector3 lookPos = PlayerController.instance.transform.position - transform.position;
            lookPos.y = 0;
            Quaternion rotation = Quaternion.LookRotation(lookPos, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 15);
        }
    }
    #endregion

    #region Custom Methods
    /// <summary>
    /// Function that calls the coroutine to handle shooting and cooldown of projectiles
    /// </summary>
    public void ShootProjectile()
    {
        StartCoroutine(Projectile());
    }
    /// <summary>
    /// Coroutine called when the projectile function is called, handles cooldowns
    /// </summary>
    /// <returns>Various wait for seconds in between cooldowns</returns>
    IEnumerator Projectile()
    {
        GameObject projectile = Instantiate(enemyProjectile, projectileSpawnPoint.position, projectileSpawnPoint.rotation);
        projectile.GetComponent<Rigidbody>().velocity = projectileSpawnPoint.forward * 5;
        behaviorState = EnemyBehaviorState.TrackingPlayer;
        yield return new WaitForSeconds(shootCooldown);
    }

    /// <summary>
    /// Function that lets this enemy strafe
    /// </summary>
    /// <param name="strafeRight"></param>
    private void Strafe(bool strafeRight = true)
    {
        Vector3 offsetPlayer;
        /**
         * Conditional calculates different vector direction, magnitude remains same
         */
        if (strafeRight)
        {
            offsetPlayer = PlayerController.instance.transform.position - transform.position;
        }
        else
        {
            offsetPlayer = transform.position - PlayerController.instance.transform.position;
        }
        /**
         * Cross product will calculate vector perpendicular to both the vector between the player
         * and the upwards vector. 
         * Result will be a vector pointing left or right relative to the direction facing the player
         * */
        Vector3 dir = Vector3.Cross(offsetPlayer, Vector3.up);
        //Move to that location that we just found
        navMeshAgent.SetDestination(transform.position + dir);
    }

    /// <summary>
    /// Function that lets this enemy move away from the player
    /// </summary>
    private void Retreat()
    {
        //Calculates a vector pointing away from the player and moves the navMesh there
        Vector3 dirToPlayer = transform.position - PlayerController.instance.transform.position;
        Vector3 runPos = transform.position + dirToPlayer;
        navMeshAgent.SetDestination(runPos);
    }

    #endregion
}