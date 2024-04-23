using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Overrides the fan mode setting of all child AudienceMembers
/// </summary>
public class AudienceMemberOverride : MonoBehaviour
{
    #region Variables
    [Header("Can call ConvertAllToPlayer() to convert all child audience members to the player's side")]

    [Tooltip("Whether to override the fan setting on child AudienceMembers")]
    [SerializeField] bool overrideAllegiance;
    [Tooltip("The fan setting to apply to child AudienceMembers")]
    [MMCondition("overrideAllegiance", Hidden = true)]
    [SerializeField] AudienceMember.FanMode allegiance;

    [Tooltip("Whether to override the enemy manager setting on child AudienceMembers")]
    [SerializeField] bool overrideEnemyManager;
    [Tooltip("The enemy manager setting to apply to child AudienceMembers")]
    [MMCondition("overrideEnemyManager", Hidden = true)]
    [SerializeField] EnemyManager enemyManager;
    #endregion

    #region Unity Methods
    /// <summary>
    /// Override settings of all child AudienceMembers
    /// </summary>
    private void Awake()
    {
        foreach (AudienceMember member in GetComponentsInChildren<AudienceMember>())
        {
            //Override allegiance
            if (overrideAllegiance)
                member.fanMode = allegiance;

            //Override enemy manager
            if (overrideEnemyManager)
                member.enemyManager = enemyManager;
        }

        
    }

    /// <summary>
    /// Can be called to convert all child audience members to the player's side
    /// </summary>
    public void ConvertAllToPlayer()
    {
        foreach (AudienceMember member in GetComponentsInChildren<AudienceMember>())
            member.StartBecomePlayerFan();
    }
    #endregion
}
