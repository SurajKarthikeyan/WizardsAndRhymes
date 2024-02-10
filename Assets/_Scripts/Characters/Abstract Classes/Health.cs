using System.Collections;
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
    [SerializeField]
    private float maximumHP;

    [Tooltip("GameObject holding the fireEffect VFX")]
    [SerializeField] 
    private GameObject fireEffect;

    [Tooltip("Boolean representing if this health is on fire")]
    [SerializeField] 
    private bool onFire;

    [Tooltip("Int representing the number of ticks from fire damage")]
    [SerializeField] 
    private int tickCount;

    [Tooltip("Float representing the time between ticks in seconds")]
    [SerializeField] 
    private float timeBetweenTicks;

    [Tooltip("Int representing the damage that the fire effect does")]
    [SerializeField] 
    private float fireDamage;

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
        onFire = false;
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
    public void TakeDamage(float value, DamageType dType)
    {
        //We should check what the damage type is when assigning effect
        if (dType == DamageType.Fire)
        {
            fireEffect.SetActive(true);
            FireDamage(tickCount);
        }
        else if (dType == DamageType.Lightning)
        {
            LightningDamage();
        }
        

        currentHP -= value;
    }

    public void LightningDamage()
    {
        Debug.Log("BZZZZZ");
    }
    
    /// <summary>
    /// Function that makes this entity take fire damage
    /// </summary>
    /// <param name="tickCount">Number of ticks of fire damage to take</param>
    public void FireDamage(int tickCount)
    {
        if (!onFire)
        {
            onFire = true;
            StartCoroutine(FireDamageCoroutine(tickCount, timeBetweenTicks, fireDamage));
        }
    }

    /// <summary>
    /// Coroutine that handles fire damage over a time interval
    /// </summary>
    /// <param name="tickCount">Number of ticks of fire damage to deal</param>
    /// <param name="timeBetweenTicks">Time in between each individual damage tick</param>
    /// <param name="fireDamage">Amount of fire damage to deal per tick</param>
    /// <returns></returns>
    IEnumerator FireDamageCoroutine(int tickCount, float timeBetweenTicks, float fireDamage)
    {
        for (int i = 0; i < tickCount; i++)
        {
            this.TakeDamage(fireDamage, DamageType.None);
            yield return new WaitForSeconds(timeBetweenTicks);
        }

        fireEffect.SetActive(false);
        onFire = false;
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
