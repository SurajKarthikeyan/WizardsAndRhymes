using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistancedProjectileEnemy : BaseEnemy
{
    #region Class Variables
    #region AI Variables
    /// <summary>
    /// NavMeshAgent component to control behavior of enemy
    /// </summary>
    UnityEngine.AI.NavMeshAgent navMeshAgent;

    /// <summary>
    /// Transform of the player to follow
    /// </summary>
    public Transform player;

    public float maxDistance = 10f;

    public float minDistance = 1f;

    public GameObject enemyProjectile;

    public Transform projectileSpawnPoint;

    public bool shooting = false;
    #endregion
    #endregion

    #region Methods
    #region Unity Methods
    /// <summary>
    /// Unity method called before the first frame update
    /// </summary>
    protected override void Start()
    {
        base.Start();
        navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
    }

    /// <summary>
    /// Unity method called every frame update
    /// </summary>
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (m_Activated)
        {
            float currDistance = Vector3.Distance(transform.position, player.transform.position);
            Debug.Log(currDistance);
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

    /// <summary>
    /// Unity method called whenever this object collides with another
    /// </summary>
    /// <param name="collision">Collision object that this enemy collides with</param>
    private void OnCollisionEnter(Collision collision)
    {
        if (m_Activated)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                collision.gameObject.GetComponent<Character>().TakeDamage(m_AttackDamage, Character.DamageType.Fire);
            }
        }
    }
    #endregion

    #region Custom Methods


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
        //m_AbilityManager.ReduceAbilityGuage(2);
        GameObject projectile = Instantiate(enemyProjectile, projectileSpawnPoint.position, projectileSpawnPoint.rotation);
        projectile.GetComponent<Rigidbody>().velocity = projectileSpawnPoint.forward * 5;
        //m_AbilityManager.ResetAbilityRecharge();
        yield return new WaitForSeconds(1f);
        //attackStatus = AttackStatus.None;
        shooting = false;
    }
    /// <summary>
    /// Helper method called upon each enemy death, allows for easy implementation
    /// of different enemy death behaviors
    /// </summary>
    protected override void EnemyDeath()
    {
        Debug.Log("FollowPlayer Death Helper");
    }
    #endregion
    #endregion
}
