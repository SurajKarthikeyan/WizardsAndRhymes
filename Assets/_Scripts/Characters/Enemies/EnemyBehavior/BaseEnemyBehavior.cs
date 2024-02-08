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
    [Tooltip("Value representing the attack damage of this enemy")]
    [SerializeField]
    protected float attackDamage = 5f;


    public bool activated = false;

    public Material activatedMaterial;
    #endregion

    [Header("Script refernces")]
    [Tooltip("Health Script Reference for this behavior")]
    private Health health;

    #region Unity Methods
    /// <summary>
    /// Function that plays on scene start
    /// </summary>
    protected virtual void Start()
    {
        health = GetComponent<BaseEnemyHealth>();
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
    #endregion
}
