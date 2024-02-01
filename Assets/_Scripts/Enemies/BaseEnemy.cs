using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Base class for enemies, all enemies derive in some way from here
/// </summary>

public class BaseEnemy : MonoBehaviour
{
    #region Class Variables
    #region Health Variables
    /// <summary>
    /// Current HP of this enemy
    /// </summary>
    [SerializeField]
    protected float m_CurrentHP;

    /// <summary>
    /// Maximum HP of this enemy
    /// </summary>
    [SerializeField]
    private float m_MaximumHP;
    #endregion

    #region Attack Variables
    /// <summary>
    /// Value representing the attack damage of this enemy
    /// </summary>
    [SerializeField]
    private float m_AttackDamage = 5f;


    public bool m_Activated = false;

    public Material m_ActivatedMaterial;
    #endregion
    #endregion

    /// <summary>
    /// C# property that allows us to access HP
    /// </summary>
    public float HP
    {
        get { return m_CurrentHP; }
        set { m_CurrentHP = value; }
    }

    protected virtual void Start()
    {
        m_CurrentHP = m_MaximumHP;
    }

    /// <summary>
    /// Unity method called every frame interval
    /// </summary>
    protected virtual void FixedUpdate()
    {
        if (m_CurrentHP <= 0)
        {
            m_Activated = false;
            EnemyDeath();
        }
    }

    /// <summary>
    /// Method that heals this enemy
    /// </summary>
    /// <param name="value">Amount to heal the enemy by</param>
    /// <param name="maxHeal">boolean stating whether or not this is a maximum heal</param>
    public void Heal(float value, bool maxHeal= false)
    {
        if (maxHeal)
        {
            m_CurrentHP = m_MaximumHP;
        }
        else { m_CurrentHP = value; }
    }

    /// <summary>
    /// Function that handles enemy death in conjunction with a helper function
    /// </summary>
    private void EnemyDeath()
    {
        EnemyDeathHelper();
        Destroy(gameObject);
    }

    /// <summary>
    /// Helper function that handles different enemy behavior upon death
    /// </summary>
    protected virtual void EnemyDeathHelper()
    {
        Debug.Log("Base BaseEnemy Death Helper");
    }
}
