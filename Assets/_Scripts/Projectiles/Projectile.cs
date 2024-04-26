using AYellowpaper.SerializedCollections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// Base class for a projectile fired by the player
/// </summary>
public class Projectile : MonoBehaviour
{
    #region Variables
    [Tooltip("Time in seconds that this projectile exists before being destroyed")]
    [SerializeField]
    private float existenceTimeThreshold = 5f;


    [Tooltip("Instance of the damage type enum")]
    [FormerlySerializedAs("DType")][SerializeField] private Health.DamageType _dType = Health.DamageType.None;
    public Health.DamageType DType
    {
        get { return _dType; }
        set { _dType = value; DamageTypeUpdated(); }
    }

    [Tooltip("Damage that this projectile deals")]
    [SerializeField]
    private int damage;

    [Header("VFX Settings")]
    [Tooltip("The projectile's renderer")]
    [SerializeField] Renderer render;
    [Tooltip("The property on the projectile's renderer that controls its color")]
    [SerializeField] string colorProperty;
    [Tooltip("The colors to use for each damage type")]
    [ColorUsage(false, hdr:true)]
    [SerializeField] List<Color> damageTypeColors;
    #endregion

    #region Unity Methods
    /// <summary>
    /// Function that calls immediately upon this scene start
    /// </summary>
    private void Awake()
    {
        Destroy(gameObject, existenceTimeThreshold);
    }

    /// <summary>
    /// Function that is called when this object's trigger collides with another collider
    /// </summary>
    /// <param name="other">Other collider that is contacted by this trigger</param>
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            if (other.TryGetComponent<Health>(out var character))
            {
                if (character.vulnerable)
                {
                    character.TakeDamage(damage, DType);
                    if (character.gameObject.TryGetComponent(out BaseEnemyBehavior enemy))
                    {
                        enemy.Knockback(transform.forward);
                    }
                }
            }

            else if (other.TryGetComponent<FirstLightningBlock>(out var lightningBlock))
            {
                lightningBlock.StartLightingChain();
            }
            Destroy(gameObject);
        }
    }
    #endregion

    #region Custom Methods
    /// <summary>
    /// Changes the color of the projectile when damage type updates
    /// </summary>
    void DamageTypeUpdated()
    {
        if ((int)DType < damageTypeColors.Count)
        {
            render.material.SetColor(colorProperty, damageTypeColors[(int)DType]);
        }
        else
            Debug.LogError("No color set for " + DType.ToString() + " type projectile");
    }
    #endregion
}
