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
    public float attackDamage = 5f;

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
    [HideInInspector]
    public NavMeshAgent navMeshAgent;

    [Tooltip("Rigidbody of this enemy")]
    protected Rigidbody rb;

    [Header("Script refernces")]
    [Tooltip("Health Script Reference for this behavior")]
    private BaseEnemyHealth health;

    [Tooltip("Health Script Reference for this behavior")]
    private AIDebug aiDebug;

    public bool hasBeenSeen;

    public bool knockedBack;

    public Vector3 knockBackDirection;

    #endregion

    #region Unity Methods

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }
    /// <summary>
    /// Function that plays on scene start
    /// </summary>
    protected virtual void Start()
    {
        health = GetComponent<BaseEnemyHealth>();
        rb = GetComponent<Rigidbody>();
        aiDebug = GetComponent<AIDebug>();
        EnemyManager.EnemiesActivated += Activate;
    }


    /// <summary>
    /// Unity method called every frame interval
    /// </summary>
    protected virtual void FixedUpdate()
    {
        if (!hasBeenSeen)
        {
            if (IsVisibleToCamera(transform))
            {
                hasBeenSeen = true;
            }
        }

        if (health.HP <= 0)
        {
            activated = false;
            StopAllCoroutines();
            health.Death();
        }
        else if (behaviorState == EnemyBehaviorState.Idle)
        {
            //Make NavMesh stay still for a certain period of time
            navMeshAgent.isStopped = true;
        }
        else
        {
            if (knockedBack)
            {
                navMeshAgent.velocity = knockBackDirection.normalized * 10;
            }
            if (navMeshAgent.isActiveAndEnabled)
            {
                navMeshAgent.isStopped = false;
            }
        }
    }

    

    /// <summary>
    /// Unity method called whenever this object collides with another
    /// </summary>
    /// <param name="collision">Collision object that this enemy collides with</param>
    protected virtual void OnCollisionEnter(Collision collision)
    {
        if (activated && hasBeenSeen)
        {
            if (collision.gameObject.TryGetComponent(out PlayerHealth playerHealth))
            {
                if (playerHealth.vulnerable)
                {
                    playerHealth.TakeDamage(attackDamage, Health.DamageType.None);
                    playerHealth.GetComponent<PlayerController>().Knockback(transform.forward, 200);
                }
                
            }
        }
    }

    /// <summary>
    /// Unity method called when this behavior is disabled
    /// </summary>
    private void OnDisable()
    {
        behaviorState = EnemyBehaviorState.Idle;
        navMeshAgent.velocity = Vector3.zero;
        aiDebug.ClearDebugText();
        activated = false;
    }

    /// <summary>
    /// Unity method called when this behavior is enabled
    /// </summary>
    private void OnEnable()
    {
        activated = true;
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

    public static bool IsVisibleToCamera(Transform transform)
    {
        Vector3 visTest = Camera.main.WorldToViewportPoint(transform.position);
        return (visTest.x >= 0 && visTest.y >= 0) && (visTest.x <= 1 && visTest.y <= 1) && visTest.z >= 0;
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
    /// Activates this enemy
    /// </summary>
    /// <param name="enemyActivated"></param>
    private void Activate(bool enemyActivated)
    {
        if (!enemyActivated)
        {
            behaviorState = EnemyBehaviorState.Idle;
        }
        activated = enemyActivated;
    }

    /// <summary>
    /// Resets the speed of the navmesh
    /// </summary>
    public void ResetNavmeshSpeed()
    {
        navMeshAgent.speed = 3.5f;
    }

    public void Knockback(Vector3 direction)
    {
        StartCoroutine(KnockbackTimer(direction));
    }

    /// <summary>
    /// Waits for the enemy to be knocked back a bit.
    /// </summary>
    /// <returns></returns>
    IEnumerator KnockbackTimer(Vector3 direction)
    {
        knockBackDirection = direction;

        float baseSpeed = navMeshAgent.speed;
        float baseAngularSpeed = navMeshAgent.angularSpeed;
        float baseAcceleration = navMeshAgent.acceleration;

        knockedBack = true;
        navMeshAgent.speed = 30;
        navMeshAgent.angularSpeed = 0;
        navMeshAgent.acceleration = 30;

        Debug.Log("Right before the .5 seconds");

        
        Debug.Log("Knocked back");
        yield return new WaitForSeconds(1f);
        knockedBack = false;

        Debug.Log("Not knocked back anymore");
        navMeshAgent.speed = baseSpeed;
        navMeshAgent.angularSpeed = baseAngularSpeed;
        navMeshAgent.acceleration = baseAcceleration;

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
