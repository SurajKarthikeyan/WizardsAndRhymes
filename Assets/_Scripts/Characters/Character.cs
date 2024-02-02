using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    #region Enums

    public enum DamageType
    {
        Fire,
        Lightning,
        Ice
    }

    #endregion
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

    #region Damage Variables
    [SerializeField] private GameObject fireEffect;
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
    /// Method that heals this enemy
    /// </summary>
    /// <param name="value">Amount to heal the enemy by</param>
    /// <param name="maxHeal">boolean stating whether or not this is a maximum heal</param>
    public void Heal(float value, bool maxHeal = false)
    {
        if (maxHeal)
        {
            m_CurrentHP = m_MaximumHP;
        }
        else { m_CurrentHP = value; }
    }

    /// <summary>
    /// Has this character take damage by the value passed in
    /// </summary>
    /// <param name="value">Float value representing the amount of damage this character takes</param>
    public void TakeDamage(float value, DamageType dType)
    {
        fireEffect.SetActive(true);
        m_CurrentHP -= value;
    }

    /// <summary>
    /// Function that handles character death in conjunction with a helper function, 
    /// different for enemies and players
    /// </summary>
    public void Death()
    {
        Destroy(gameObject);
    }

    
}
