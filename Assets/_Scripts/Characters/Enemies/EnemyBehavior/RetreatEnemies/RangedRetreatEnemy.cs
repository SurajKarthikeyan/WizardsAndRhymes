using System.Collections;
using UnityEngine;

/// <summary>
/// Ranged variant of the retreating enemy
/// </summary>
public class RangedRetreatEnemy : BaseRetreatEnemy
{
    #region Variables
    [Header("Projectile variables")]
    [Tooltip("Projectile object that this enemy shoots")]
    [SerializeField]
    private GameObject enemyProjectile;

    [Tooltip("Transform that this enemy shoots the projectile from")]
    [SerializeField]
    private Transform projectileSpawnPoint;

    [Tooltip("Cooldown in between every projectile shot by this enemy")]
    [SerializeField]
    private float shootCooldown = 1f;
    #endregion

    #region Unity Methods
    /// <summary>
    /// Unity method called before the first frame update
    /// </summary>
    protected override void Start()
    {
        base.Start();
    }

    /// <summary>
    /// Unity method called every frame update
    /// </summary>
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (activated && hasBeenSeen)
        {
            LookAtPlayer();
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
        navMeshAgent.enabled = true;
    }
    /// <summary>
    /// Coroutine called when the projectile function is called, handles cooldowns
    /// </summary>
    /// <returns>Various wait for seconds in between cooldowns</returns>
    IEnumerator Projectile()
    {
        GameObject projectile = Instantiate(enemyProjectile, projectileSpawnPoint.position, projectileSpawnPoint.rotation);
        projectile.GetComponent<Rigidbody>().velocity = projectileSpawnPoint.forward * 5;
        yield return new WaitForSeconds(shootCooldown);
    }

    /// <summary>
    /// Evaluates the current distance from the player and determines the next behavior state
    /// </summary>
    protected override void EvaluateDistanceFromPlayer()
    {
        float currDistance = Vector3.Distance(transform.position, PlayerController.instance.transform.position);
        //If we are retreating after the projectile attack
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
                if (navMeshAgent.remainingDistance <= .5f)
                {
                    StartCoroutine(PausePlayerTracking(1f));
                    navMeshAgent.speed = originalSpeed;
                }
            }
        }
        //This is when we are following the player at a great distance
        else if (currDistance > maxDistance && behaviorState != EnemyBehaviorState.RangedAttacking)
        {
            behaviorState = EnemyBehaviorState.TrackingPlayer;
            navMeshAgent.destination = PlayerController.instance.transform.position;
        }
        //If the enemy is too close to the player
        else if (currDistance < minDistance && behaviorState != EnemyBehaviorState.RangedAttacking)
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
    /// Special behavior of this enemy: retreat and shoot a projectile
    /// </summary>
    protected override void SpecializedBehavior()
    {
        //If the enemy is tracking the player and in range, we begin to attack
        if (behaviorState == EnemyBehaviorState.TrackingPlayer)
        {
            behaviorState = EnemyBehaviorState.RangedAttacking;
        }
        //If the enemy is ready to attack
        else if (behaviorState == EnemyBehaviorState.RangedAttacking)
        {
            navMeshAgent.enabled = false;
            ShootProjectile();
            behaviorState = EnemyBehaviorState.Retreating;
        }
    }
    #endregion
}
