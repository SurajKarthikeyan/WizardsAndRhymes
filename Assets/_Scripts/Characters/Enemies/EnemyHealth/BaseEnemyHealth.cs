using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Base class that handles all enemy health, status effects, and death
/// </summary>
public abstract class BaseEnemyHealth : Health
{
    #region Variables
    
    [Header("Fire")]
    [Tooltip("GameObject holding the fireEffect VFX")]
    [SerializeField] 
    private GameObject fireEffect;

    [Tooltip("Boolean representing if this health is on fire")]
    [SerializeField] 
    private bool onFire;

    [Tooltip("Int representing the number of ticks from fire damage")]
    [SerializeField] 
    private int fireTickCount;

    [Tooltip("Float representing the time between ticks in seconds")]
    [SerializeField] 
    private float fireTimeBetweenTicks;

    [Tooltip("Int representing the damage that the fire effect does")]
    [SerializeField] 
    private float fireDamage;

    [Header("Lightning")]
    [Tooltip("Duration of lightning status")]
    [SerializeField] private int lightningDuration;
    
    [Tooltip("Status of being on lightning")]
    [SerializeField] private bool onLightning;
    
    [Tooltip("Prefab to spawn lightning effect")]
    [SerializeField] private GameObject lightningEffectPrefab;
    
    [Tooltip("Distance to which the enemies should chain to")]
    [SerializeField] private float lightningChainDistance;

    [Tooltip("Layer mask to mask enemies on lightning hit")]
    [SerializeField] private LayerMask enemyLayerMask;

    [Tooltip("Base damage when chained to or when hit with lighitning")]
    [SerializeField] private int lightningBaseDamage;

    [Tooltip("Used to store the vfx of the lightning. If the enemy dies, can be easily cleared")]
    [HideInInspector] private List<GameObject> lightiningEffectStorage;
    
    [Header("Ice")]
    [Tooltip("Multiplier to reduce enemy speed of when hit with ice damage")]
    [SerializeField] private float iceSpeedDecreaseMultipler;
    
    [Tooltip("Reference of the original enemy speed")]
    [HideInInspector] private float originalEnemySpeed;
    
    [Tooltip("Slowed enemy speed")]
    [HideInInspector] private float slowedEnemySpeed;

    [Tooltip("Duration to be frozen")]
    [SerializeField] private float iceDuration;

    [Tooltip("Bool on if the enemy is forzen")]
    [SerializeField] private bool onIce;

    [Tooltip("Base damage that enemy takes when hit with ice attack")]
    [SerializeField] private int iceBaseDamage;
    
    [Tooltip("Base enemy behaviour reference for status settings")]
    [SerializeField] private BaseEnemyBehavior enemyBehavior;
    
    [Tooltip("Audio Event for skeleton hit")]
    [SerializeField] private AK.Wwise.Event enemyHitSFX;

    public Image healthBar;

    public delegate void EnemyDiedDelegate(GameObject enemyGO);
    [Tooltip("Event fired when an enemy dies")]
    [HideInInspector] public static event EnemyDiedDelegate EnemyDied;

    [Tooltip("Boolean stating if this enemy is dead")]
    private bool isDead;
    #endregion


    #region Unity Methods

    protected override void Start()
    {
        base.Start();
        onFire = false;
        isDead = false;
        lightiningEffectStorage = new List<GameObject>();
    }

    private void Update()
    {
        healthBar.fillAmount = currentHP / maximumHP;
    }

    #endregion
    
    
    #region Custom Methods
    /// <summary>
    /// Death method that is overridden for all characters with health
    /// </summary>
    public override void Death()
    {
        isDead = true;
        EnemyDeath();
    }

    #region DamageMethods

    /// <summary>
    /// Function that makes the enemy take damage
    /// 
    /// </summary>
    /// <param name="value"></param>
    /// <param name="dType"></param>
     public override void TakeDamage(float value, DamageType dType)
    {
        if (!isDead && vulnerable)
        {
            enemyHitSFX.Post(this.gameObject);
            //We should check what the damage type is when assigning effect
            if (dType == DamageType.Fire)
            {
                fireEffect.SetActive(true);
                FireDamage(fireTickCount);
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
        
    }
    
    
    /// <summary>
    /// Function representing taking lightning damage
    /// </summary>
    public void LightningDamage()
    {
        Debug.Log("Lightning");
        Collider[] enemyLightiningCollider = Physics.OverlapSphere(this.gameObject.transform.position,
            lightningChainDistance, enemyLayerMask);
        Collider[] eletricBlockCollider = Physics.OverlapSphere(this.gameObject.transform.position,
            lightningChainDistance);
        for (int i = 0; i < enemyLightiningCollider.Length; i++)
        {
            if (enemyLightiningCollider[i].gameObject.TryGetComponent(out BaseEnemyHealth curEnemyHealth))
            {
                if (curEnemyHealth.vulnerable)
                {
                    Vector3 deltaPos = enemyLightiningCollider[i].gameObject.transform.position -
                                     this.gameObject.transform.position;
                    // Take Damage
                    curEnemyHealth.TakeDamage(lightningBaseDamage, DamageType.None);
                    
                    //Draw lightning arc
                    GameObject curLightningEffect = Instantiate(lightningEffectPrefab);
                    curLightningEffect.transform.position = PlayerController.instance.transform.position;
                    
                    //set start to this object
                    curLightningEffect.GetComponent<LightningVFXPosition>().pos1.transform.position =
                        this.gameObject.transform.position;
                    curLightningEffect.GetComponent<LightningVFXPosition>().pos1.transform.parent =
                        this.gameObject.transform;  // Set start as child to this object
                    
                    //set end to enemy object chained
                    curLightningEffect.GetComponent<LightningVFXPosition>().pos4.transform.position =
                        enemyLightiningCollider[i].gameObject.transform.position;

                    curLightningEffect.GetComponent<LightningVFXPosition>().pos4.transform.parent =
                        enemyLightiningCollider[i].gameObject.transform;    // set end point to child of parent

                    Vector3 pos2 = new Vector3(
                        curLightningEffect.GetComponent<LightningVFXPosition>().pos1.transform.position.x + deltaPos.x * 0.33f,
                        curLightningEffect.GetComponent<LightningVFXPosition>().pos1.transform.position.y + deltaPos.y * 0.33f,
                        curLightningEffect.GetComponent<LightningVFXPosition>().pos1.transform.position.z + deltaPos.z * 0.33f);
                    
                    Vector3 pos3 = new Vector3(
                        curLightningEffect.GetComponent<LightningVFXPosition>().pos1.transform.position.x + deltaPos.x * 0.66f,
                        curLightningEffect.GetComponent<LightningVFXPosition>().pos1.transform.position.y + deltaPos.y * 0.66f,
                        curLightningEffect.GetComponent<LightningVFXPosition>().pos1.transform.position.z + deltaPos.z * 0.66f);

                    curLightningEffect.GetComponent<LightningVFXPosition>().pos2.transform.position = pos2;
                    curLightningEffect.GetComponent<LightningVFXPosition>().pos3.transform.position = pos3;
                    
                    
                    lightiningEffectStorage.Add(curLightningEffect);
                }
            }
        }

       /* for (int i = 0; i < eletricBlockCollider.Length; i++)
        {
            if(eletricBlockCollider[i].gameObject.TryGetComponent(out FirstLightningBlock firstLightningBlock))
            {
                    Vector3 deltaPos = eletricBlockCollider[i].gameObject.transform.position -
                                       this.gameObject.transform.position;
                    
                    //Draw lightning arc
                    GameObject curLightningEffect = Instantiate(lightningEffectPrefab);
                    curLightningEffect.transform.position = PlayerController.instance.transform.position;
                    
                    //set start to this object
                    curLightningEffect.GetComponent<LightningVFXPosition>().pos1.transform.position =
                        this.gameObject.transform.position;
                    curLightningEffect.GetComponent<LightningVFXPosition>().pos1.transform.parent =
                        this.gameObject.transform;  // Set start as child to this object
                    
                    //set end to enemy object chained
                    curLightningEffect.GetComponent<LightningVFXPosition>().pos4.transform.position =
                        eletricBlockCollider[i].gameObject.transform.position;

                    curLightningEffect.GetComponent<LightningVFXPosition>().pos4.transform.parent =
                        eletricBlockCollider[i].gameObject.transform;    // set end point to child of parent

                    Vector3 pos2 = new Vector3(
                        curLightningEffect.GetComponent<LightningVFXPosition>().pos1.transform.position.x + deltaPos.x * 0.33f,
                        curLightningEffect.GetComponent<LightningVFXPosition>().pos1.transform.position.y + deltaPos.y * 0.33f,
                        curLightningEffect.GetComponent<LightningVFXPosition>().pos1.transform.position.z + deltaPos.z * 0.33f);
                    
                    Vector3 pos3 = new Vector3(
                        curLightningEffect.GetComponent<LightningVFXPosition>().pos1.transform.position.x + deltaPos.x * 0.66f,
                        curLightningEffect.GetComponent<LightningVFXPosition>().pos1.transform.position.y + deltaPos.y * 0.66f,
                        curLightningEffect.GetComponent<LightningVFXPosition>().pos1.transform.position.z + deltaPos.z * 0.66f);

                    curLightningEffect.GetComponent<LightningVFXPosition>().pos2.transform.position = pos2;
                    curLightningEffect.GetComponent<LightningVFXPosition>().pos3.transform.position = pos3;
                    
                    eletricBlockCollider[i].gameObject.GetComponent<FirstLightningBlock>().StartLightingChain();
                    lightiningEffectStorage.Add(curLightningEffect);
            }
        }*/

        StartCoroutine(LightingArc());
        
        if (vulnerable)
        {
            this.TakeDamage(lightningBaseDamage, DamageType.None);
        }
    }


    /// <summary>
    /// Coroutine to wait to show the lightning and then destroy it
    /// </summary>
    /// <returns></returns>
    IEnumerator LightingArc()
    {
        yield return new WaitForSeconds(0.5f);
        ClearLightningObjects();
    }

    /// <summary>
    /// Function to clear all lightning effects in scene
    /// </summary>
    public void ClearLightningObjects()
    {
        foreach (GameObject go in lightiningEffectStorage)
        {
            Destroy(go);
        }
    }

    /// <summary>
    /// Function representing taking Ice damage
    /// </summary>
    public void IceDamage()
    {
        if (vulnerable)
        {
            Debug.Log("Ice");
            TakeDamage(iceBaseDamage, DamageType.None);
            onIce = true;
            enemyBehavior.behaviorState = BaseEnemyBehavior.EnemyBehaviorState.Ice;
            StartCoroutine(IceDamageCoroutine());
        }
    }

    /// <summary>
    /// Coroutine that sets enemy status to ice
    /// </summary>
    /// <returns></returns>
    IEnumerator IceDamageCoroutine()
    {
        yield return new WaitForSeconds(iceDuration);
        onIce = false;
        enemyBehavior.behaviorState = BaseEnemyBehavior.EnemyBehaviorState.Idle;
        enemyBehavior.ResetNavmeshSpeed();
    }
    
    /// <summary>
    /// Function that makes this entity take fire damage
    /// </summary>
    /// <param name="tickCount">Number of ticks of fire damage to take</param>
    public void FireDamage(int tickCount)
    {
        
        if (!onFire && vulnerable)
        {
            Debug.Log("Fire");
            onFire = true;
            StartCoroutine(FireDamageCoroutine(tickCount, fireTimeBetweenTicks, fireDamage));
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
            if (vulnerable)
            {
                this.TakeDamage(fireDamage, DamageType.None);
                yield return new WaitForSeconds(timeBetweenTicks);
            }
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
        StopAllCoroutines();
        ClearLightningObjects();
        EnemyDied?.Invoke(gameObject);
    }
    #endregion
}
