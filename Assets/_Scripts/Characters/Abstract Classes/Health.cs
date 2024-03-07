using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

/// <summary>
/// Class that represents a base health
/// </summary>
public class Health : MonoBehaviour
{
    #region Variables
    [Tooltip("Enum representing the different damage types an entity can take")]
    public enum DamageType
    {
        Fire,
        Lightning,
        Ice,
        None
    }

    [Tooltip("Current HP of this enemy")]
    [SerializeField]
    protected float currentHP;

    [Tooltip("Maximum HP of this enemy")]
    
    public float maximumHP;

    public bool vulnerable;
    

    [Tooltip("C# property that allows us to access HP")]
    public float HP
    {
        get { return currentHP; }
        set { currentHP = value; }
    }
    #endregion

    #region Unity Methods
    /// <summary>
    /// Method that is called on scene load
    /// </summary>
    protected virtual void Start()
    {
        currentHP = maximumHP;
    }
    #endregion

    #region Custom Methods

    /// <summary>
    /// Method that heals this enemy
    /// </summary>
    /// <param name="value">Amount to heal the enemy by</param>
    /// <param name="maxHeal">boolean stating whether or not this is a maximum heal</param>
    public void Heal(float value, bool maxHeal = false)
    {
        if (maxHeal)
        {
            currentHP = maximumHP;
        }
        else { currentHP = value; }
    }

    
    /// <summary>
    /// Has this health take damage by the value passed in
    /// </summary>
    /// <param name="value">Float value representing the amount of damage this health takes</param>
    public virtual void TakeDamage(float value, DamageType dType)
    {
        currentHP -= value;
    }

    
    /// <summary>
    /// Function that handles health death in conjunction with a helper function, 
    /// different for enemies and players
    /// </summary>
    public virtual void Death()
    {
        Destroy(gameObject);
    }
    #endregion

}
