using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Base class for enemies, all enemies derive in some way from here
/// </summary>

public abstract class BaseEnemyBehavior : MonoBehaviour
{
    #region Variables
    
    /// <summary>
    /// Value representing the attack damage of this enemy
    /// </summary>
    [SerializeField]
    protected float attackDamage = 5f;


    public bool activated = false;

    public Material activatedMaterial;
    #endregion

    #region Script References
    private Health health;
    #endregion 


    protected virtual void Start()
    {
        health = GetComponent<Health>();
    }
    /// <summary>
    /// Unity method called every frame interval
    /// </summary>
    protected virtual void FixedUpdate()
    {
        if (health.HP <= 0)
        {
            activated = false;
            EnemyDeath();
            health.Death();
        }
    }

    /// <summary>
    /// Helper function that handles different enemy behavior upon death
    /// </summary>
    protected virtual void EnemyDeath()
    {
        Debug.Log("Base BaseEnemyBehavior Death Helper");
    }


}
