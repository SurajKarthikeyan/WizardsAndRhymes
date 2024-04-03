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
        //Override allegiance
        if (overrideAllegiance)
        {
            foreach (AudienceMember member in GetComponentsInChildren<AudienceMember>())
                member.fanMode = allegiance;
        }

        //Override enemy manager
        if (overrideEnemyManager)
        {
            foreach (AudienceMember member in GetComponentsInChildren<AudienceMember>())
                member.enemyManager = enemyManager;
        }
    }
    #endregion
}
