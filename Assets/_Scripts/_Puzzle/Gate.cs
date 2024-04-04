using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    [SerializeField] private Animator gateAnim;
    [SerializeField] private BoxCollider gateCollider;
    public void SwitchGate()
    {
        gateAnim.SetTrigger("interaction");
        if (gateCollider.enabled == false)
        {
            gateCollider.enabled = true;
        }
        else
        {
            gateCollider.enabled = false;
        }
    }
}
