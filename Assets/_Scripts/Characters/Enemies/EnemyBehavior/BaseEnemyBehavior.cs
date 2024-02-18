using System.Collections;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Base class for enemies, all enemies derive in some way from here
/// </summary>

public abstract class BaseEnemyBehavior : MonoBehaviour
{
    #region Variables
    [Tooltip("Enum representing the different states of behavior an enemy can be in")]
    public enum EnemyBehaviorState
    {
        Idle,
        TrackingPlayer,
        Strafing,
        MeleeAttacking,
        RangedAttacking,
        Retreating,
        Ice
    }
    [Header("Enemy Behavior State")]
    [Tooltip("Instance of the EnemyBehaviorState enum")]
    [HideInInspector]
    public EnemyBehaviorState behaviorState;

    [Header("Attack Variables")]
    [Tooltip("Value representing the attack damage of this enemy")]
    [SerializeField]
    protected float attackDamage = 5f;

    [Header("Distancing variables")]
    [Tooltip("Maximum distance this enemy will be from the player before it shoots")]
    [SerializeField]
    protected float maxDistance = 10f;

    [Tooltip("Minimum distance this enemy will be from the player before it shoots")]
    [SerializeField]
    protected float minDistance = 1f;

    [Header("Activation Variables")]
    [Tooltip("Boolean representing if this enemy is active or not")]
    public bool activated = false;

    [Tooltip("Material applied to the enemy when activated, primarily used for early testing purposes")]
    public Material activatedMaterial;

    [Header("Navigation/Movement Variables")]
    [Tooltip("NavMeshAgent that is enemy behavior uses for its general navigation")]
    protected NavMeshAgent navMeshAgent;

    [Tooltip("Rigidbody of this enemy")]
    protected Rigidbody rb;

    [Header("Script refernces")]
    [Tooltip("Health Script Reference for this behavior")]
    private BaseEnemyHealth health;

    #endregion

    #region Unity Methods
    /// <summary>
    /// Function that plays on scene start
    /// </summary>
    protected virtual void Start()
    {
        health = GetComponent<BaseEnemyHealth>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
    }

    /// <summary>
    /// Unity method called every frame interval
    /// </summary>
    protected virtual void FixedUpdate()
    {
        if (health.HP <= 0)
        {
            activated = false;
            StopAllCoroutines();
            health.Death();
        }
        
        else if (behaviorState == EnemyBehaviorState.Ice)
        {
            //hehe haha
        }
        else if (behaviorState == EnemyBehaviorState.Idle)
        {
            //Make NavMesh stay still for a certain period of time
            navMeshAgent.isStopped = true;
        }
        else
        {
            if (navMeshAgent.isActiveAndEnabled)
            {
                navMeshAgent.isStopped = false;
            }
        }
    }

    public void ResetNavmeshSpeed()
    {
        navMeshAgent.speed = 3.5f;
    }

    /// <summary>
    /// Unity method called whenever this object collides with another
    /// </summary>
    /// <param name="collision">Collision object that this enemy collides with</param>
    protected virtual void OnCollisionEnter(Collision collision)
    {
        if (activated)
        {
            if (collision.gameObject.TryGetComponent(out PlayerHealth playerHealth))
            {
                playerHealth.TakeDamage(attackDamage, Health.DamageType.None);
            }
        }
    }
    #endregion

    #region Custom Methods
    /// <summary>
    /// Function that faces this enemy in the direction of the player
    /// </summary>
    protected void LookAtPlayer()
    {
        //Rest of the code calculates rotation that the enemy is looking in and smoothly rotates it
        Vector3 lookPos = PlayerController.instance.transform.position - transform.position;
        lookPos.y = 0;
        Quaternion rotation = Quaternion.LookRotation(lookPos, Vector3.up);
        transform.rotation = rotation;
    }

    /// <summary>
    /// Function that lets this enemy move away from the player a small amount
    /// </summary>
    protected void SmallRetreat()
    {
        //Calculates a vector pointing away from the player and moves the navMesh there
        Vector3 dirToPlayer = transform.position - PlayerController.instance.transform.position;
        Vector3 runPos = transform.position + dirToPlayer;
        navMeshAgent.SetDestination(runPos);
    }


    /// <summary>
    /// Virtual function that evaluates the current behavior status of this enemy and decides its next state
    /// </summary>
    protected virtual void EvaluateBehavior()
    {
        Debug.Log("BaseEnemyBehavior EvaluateBehavior");
    }

    /// <summary>
    /// Virtual function called as a part of EvaluateBehavior, and it calculates the distance from
    /// the player, and determines what the next course of action should be
    /// </summary>
    protected virtual void EvaluateDistanceFromPlayer()
    {
        Debug.Log("BaseEnemyBehavior EvaluateDistanceFromPlayer");
    }

    /// <summary>
    /// Virtual function that each enemy type implements, and it allows each enemy's 
    /// behavior to be executed
    /// </summary>
    protected virtual void SpecializedBehavior()
    {
        Debug.Log("BaseEnemyBehavior has no specialized behavior");
    }

    /// <summary>
    /// Coroutine that stops the enemy from moving for a certain period of time
    /// </summary>
    /// <param name="seconds">How long the enemy stops moving</param>
    /// <returns>A WaitForSeconds as long as the paramter passed in</returns>
    protected IEnumerator PausePlayerTracking(float seconds)
    {
        behaviorState = EnemyBehaviorState.Idle;
        yield return new WaitForSeconds(seconds);
        behaviorState = EnemyBehaviorState.TrackingPlayer;
    }
    #endregion

}
