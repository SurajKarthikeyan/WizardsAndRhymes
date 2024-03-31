using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Overrides the fan mode setting of all child AudienceMembers
/// </summary>
public class CrowdTileAllegianceOverride : MonoBehaviour
{
    #region Variables
    [Tooltip("Whether to override the fan setting on child AudienceMembers")]
    [SerializeField] bool overrideAllegiance = true;
    [Tooltip("The fan setting to apply to child AudienceMembers")]
    [SerializeField] AudienceMember.FanMode allegiance;
    #endregion

    #region Unity Methods
    /// <summary>
    /// Override the allegiance of all child AudienceMembers
    /// </summary>
    private void Awake()
    {
        if (overrideAllegiance)
        {
            foreach (AudienceMember member in GetComponentsInChildren<AudienceMember>())
                member.fanMode = allegiance;
        }
    }
    #endregion
}
