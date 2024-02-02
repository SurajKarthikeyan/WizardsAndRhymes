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
    #region Attack Variables
    /// <summary>
    /// Value representing the attack damage of this enemy
    /// </summary>
    [SerializeField]
    protected float m_AttackDamage = 5f;


    public bool m_Activated = false;

    public Material m_ActivatedMaterial;
    #endregion

    #region Script References
    private Character m_Character;
    #endregion 
    #endregion


    protected virtual void Start()
    {
        m_Character = GetComponent<Character>();
    }
    /// <summary>
    /// Unity method called every frame interval
    /// </summary>
    protected virtual void FixedUpdate()
    {
        if (m_Character.HP <= 0)
        {
            m_Activated = false;
            EnemyDeath();
            m_Character.Death();
        }
    }

    /// <summary>
    /// Helper function that handles different enemy behavior upon death
    /// </summary>
    protected virtual void EnemyDeath()
    {
        Debug.Log("Base BaseEnemy Death Helper");
    }


}
