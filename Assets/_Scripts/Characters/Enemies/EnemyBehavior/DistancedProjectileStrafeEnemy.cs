using System.Collections;
using UnityEngine;

/// <summary>
/// Enemy that will shoot a projectile when in range and strafe before shooting another
/// </summary>
public class DistancedProjectileStrafeEnemy : BaseEnemyBehavior
{
    #region Variables
    [Tooltip("Transform of the player to follow")]
    [SerializeField]
    private Transform player;

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

    [Tooltip("Boolean stating whether or not this enemy is currently shooting")]
    private bool shooting = false;
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
        if (activated)
        {
            float currDistance = Vector3.Distance(transform.position, player.transform.position);
            if (currDistance > maxDistance)
            {
                navMeshAgent.destination = player.position;
            }
            else if (currDistance < minDistance)
            {
                Vector3 dirToPlayer = transform.position - player.transform.position;
                Vector3 runPos = transform.position + dirToPlayer;
                navMeshAgent.SetDestination(runPos);
            }
            else if (currDistance < maxDistance && currDistance > minDistance)
            {
                navMeshAgent.velocity = Vector3.zero;
                Vector3 lookDir = player.transform.position - transform.position;
                navMeshAgent.transform.rotation = Quaternion.LookRotation(lookDir, Vector3.up);

                if (!shooting)
                {
                    ShootProjectile();
                }
            }
        }
    }
    #endregion

    #region Custom Methods
    /// <summary>
    /// Function that calls the coroutine to handle shooting and cooldown of projectiles
    /// </summary>
    public void ShootProjectile()
    {
        shooting = true;
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
        yield return new WaitForSeconds(shootCooldown);
        shooting = false;
    }
    #endregion

}
