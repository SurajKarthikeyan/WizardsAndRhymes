using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    [SerializeField] private Animator gateAnim;

    public void SwitchGate()
    {
        gateAnim.SetTrigger("interaction");
    }
}
