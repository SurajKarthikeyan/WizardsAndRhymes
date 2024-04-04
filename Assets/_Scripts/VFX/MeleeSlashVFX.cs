using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the slash effect on the player's melee attack
/// </summary>
public class MeleeSlashVFX : MonoBehaviour
{
    [Tooltip("The particle system responsible for the melee slash VFX")]
    [SerializeField] ParticleSystem particles;
    [Tooltip("The particle renderer responsible for the melee slash VFX")]
    [SerializeField] ParticleSystemRenderer particleRenderer;
    [Tooltip("The color property to set on the particle renderer's material")]
    [SerializeField] string colorProperty;
    [Tooltip("The MeleeCollider script for the melee attack")]
    [SerializeField] MeleeCollider meleeCollider;
    [Tooltip("The colors to use for each damage type")]
    [ColorUsage(true, true)]
    [SerializeField] List<Color> damageTypeColors;

    /// <summary>
    /// Assign the correct color to the slash VFX and play it
    /// </summary>
    public void PlayVFX()
    {
        particleRenderer.trailMaterial.SetColor(colorProperty, damageTypeColors[(int)meleeCollider.damageType]);
        particles.Play();
    }
}
