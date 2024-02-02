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
    [SerializeField] private bool onFire;
    [SerializeField] private int tickCount;
    [SerializeField] private float timeBetweenTicks;
    [SerializeField] private float fireDamage;
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
        onFire = false;
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
        //We should check what the damage type is when assigning effect
        fireEffect.SetActive(true);
        FireDamage(tickCount);
        m_CurrentHP -= value;
    }

    /// <summary>
    /// Overloaded damage function with no consideration of the type of damage(used for tick damage)
    /// </summary>
    /// <param name="value"></param>
    public void TakeDamage(float value)
    {
        m_CurrentHP -= value;
    }

    public void FireDamage(int tickCount)
    {
        if (!onFire)
        {
            onFire = true;
            StartCoroutine(FireDamageCoroutine(tickCount, timeBetweenTicks, fireDamage));
        }
    }

    IEnumerator FireDamageCoroutine(int tickCount, float timeBetweenTicks, float fireDamage)
    {
        for (int i = 0; i < tickCount; i++)
        {
            this.TakeDamage(fireDamage);
            yield return new WaitForSeconds(timeBetweenTicks);
        }

        fireEffect.SetActive(false);
        onFire = false;
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
