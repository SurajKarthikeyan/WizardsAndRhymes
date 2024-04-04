using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAnimationEvent : MonoBehaviour
{
    [SerializeField] private Collider meleeBoxCollider;

    [Tooltip("The melee attack slash VFX script")]
    [SerializeField] MeleeSlashVFX meleeSlashVFX;

    public void EnableBoxCollider()
    {
        meleeBoxCollider.enabled = true;
    }

    public void DisableBoxCollider()
    {
        meleeBoxCollider.enabled = false;
    }

    public void PlaySlashParticles()
    {
        meleeSlashVFX.PlayVFX();
    }
}
