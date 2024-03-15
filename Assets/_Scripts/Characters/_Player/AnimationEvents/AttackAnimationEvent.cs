using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAnimationEvent : MonoBehaviour
{
    [SerializeField] private Collider meleeBoxCollider;

    public void EnableBoxCollider()
    {
        meleeBoxCollider.enabled = true;
    }

    public void DisableBoxCollider()
    {
        meleeBoxCollider.enabled = false;
    }
}
