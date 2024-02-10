using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Base class that handles all enemy health, status effects, and death
/// </summary>
public abstract class BaseEnemyHealth : Health
{
    #region Variables
    
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
    

    #endregion


    #region Unity Methods

    protected override void Start()
    {
        base.Start();
        onFire = false;
    }

    #endregion
    
    
    #region Custom Methods
    /// <summary>
    /// Death method that is overridden for all characters with health
    /// </summary>
    public override void Death()
    {
        EnemyDeath();
    }

    #region DamageMethods

     public override void TakeDamage(float value, DamageType dType)
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
        
        else if (dType == DamageType.Ice)
        {
            IceDamage();
        }
        base.TakeDamage(value, dType);
        
    }
    
    
    /// <summary>
    /// Function representing taking lightning damage
    /// </summary>
    public void LightningDamage()
    {
        Debug.Log("ZEUS");
    }

    /// <summary>
    /// Function representing taking Ice damage
    /// </summary>
    public void IceDamage()
    {
        Debug.Log("ICE KING");
    }
    
    /// <summary>
    /// Function that makes this entity take fire damage
    /// </summary>
    /// <param name="tickCount">Number of ticks of fire damage to take</param>
    public void FireDamage(int tickCount)
    {
        
        Debug.Log("FIRE");
        /*if (!onFire)
        {
            onFire = true;
            StartCoroutine(FireDamageCoroutine(tickCount, timeBetweenTicks, fireDamage));
        }*/
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
    /// Death method here is generalized to ensure that the script is removed from the manager
    /// </summary>

    #endregion
   
    protected virtual void EnemyDeath()
    {
        EnemyManager.enemyManager.enemies.Remove(this.gameObject.GetComponent<BaseEnemyBehavior>());

        /*Debug.Log("BaseEnemyDeath");
        Destroy(gameObject);*/
    }
    #endregion
}
