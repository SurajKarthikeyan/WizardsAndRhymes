using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Base class for enemies, all enemies derive in some way from here
/// </summary>

public abstract class BaseEnemyBehavior : MonoBehaviour
{
    #region Variables
    [Tooltip("Enum representing the different states of behavior an enemy can be in")]
    protected enum EnemyBehaviorState
    {
        Inactive,
        TrackingPlayer,
        Strafing,
        MeleeAttacking,
        RangedAttacking,
        Retreating
    }
    [Header("Enemy Behavior State")]
    [Tooltip("Instance of the EnemyBehaviorState enum")]
    [SerializeField]
    protected EnemyBehaviorState behaviorState;

    [Header("Attack Variables")]
    [Tooltip("Value representing the attack damage of this enemy")]
    [SerializeField]
    protected float attackDamage = 5f;

    [Header("Activation Variables")]
    [Tooltip("Boolean representing if this enemy is active or not")]
    public bool activated = false;

    [Tooltip("Material applied to the enemy when activated, primarily used for early testing purposes")]
    public Material activatedMaterial;

    [Header("Navigation Variables")]
    [Tooltip("NavMeshAgent that is enemy behavior uses for its general navigation")]
    protected NavMeshAgent navMeshAgent;

    [Header("Script refernces")]
    [Tooltip("Health Script Reference for this behavior")]
    private BaseEnemyHealth health;


    [Header("Distancing variables")]
    [Tooltip("Maximum distance this enemy will be from the player before it shoots")]
    [SerializeField]
    protected float maxDistance = 10f;

    [Tooltip("Minimum distance this enemy will be from the player before it shoots")]
    [SerializeField]
    protected float minDistance = 1f;

    #endregion

    #region Custom Methods
    protected void LookAtPlayer()
    {
        //Rest of the code calculates rotation that the enemy is looking in and smoothly rotates it
        Vector3 lookPos = PlayerController.instance.transform.position - transform.position;
        lookPos.y = 0;
        Quaternion rotation = Quaternion.LookRotation(lookPos, Vector3.up);
        transform.rotation = rotation;
    }

    /// <summary>
    /// Function that lets this enemy move away from the player
    /// </summary>
    protected void SmallRetreat()
    {
        //Calculates a vector pointing away from the player and moves the navMesh there
        Vector3 dirToPlayer = transform.position - PlayerController.instance.transform.position;
        Vector3 runPos = transform.position + dirToPlayer;
        navMeshAgent.SetDestination(runPos);
    }


    protected void EvaluateBehavior()
    {

    }

    protected void EvaluateDistance()
    {

    }

    protected virtual void SpecializedBehavior()
    {
        Debug.Log("BaseEnemyBehavior has no specialized behavior");
    }
    #endregion

    #region Unity Methods
    /// <summary>
    /// Function that plays on scene start
    /// </summary>
    protected virtual void Start()
    {
        health = GetComponent<BaseEnemyHealth>();
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    /// <summary>
    /// Unity method called every frame interval
    /// </summary>
    protected virtual void FixedUpdate()
    {
        if (health.HP <= 0)
        {
            activated = false;
            health.Death();
        }
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
}
