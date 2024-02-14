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
        RangedAttacking
    }
    [Header("Enemy Behavior State")]
    [Tooltip("Instance of the EnemyBehaviorState enum")]
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

    [Tooltip("Transform of the player to follow")]
    [SerializeField]
    protected Transform player;

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
