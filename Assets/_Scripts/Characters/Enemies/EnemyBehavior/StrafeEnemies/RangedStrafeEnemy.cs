using System.Collections;
using UnityEngine;

/// <summary>
/// Enemy that will shoot a projectile when in range and strafe before shooting another
/// </summary>
public class RangedStrafeEnemy : BaseStrafeEnemy
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
        //Instantiates and shoots projectile 
        GameObject projectile = Instantiate(enemyProjectile, projectileSpawnPoint.position, projectileSpawnPoint.rotation);
        projectile.GetComponent<Rigidbody>().velocity = projectileSpawnPoint.forward * 10;
        behaviorState = EnemyBehaviorState.TrackingPlayer;
        yield return new WaitForSeconds(shootCooldown);
    }

    /// <summary>
    /// Function override that executes this enemy's special behavior: 
    /// Strafes around the player when within a certain range and shoots them
    /// </summary>
    protected override void SpecializedBehavior()
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
            ShootProjectile();
        }
    }
    #endregion
}
