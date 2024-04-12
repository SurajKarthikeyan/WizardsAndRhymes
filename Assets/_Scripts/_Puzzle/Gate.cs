using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    [SerializeField] private Animator gateAnim;
    [SerializeField] private AK.Wwise.Event gateSoundEffect;
    public virtual void SwitchGate()
    {
        gateAnim.SetTrigger("interaction");
        gateSoundEffect.Post(this.gameObject);
    }

    // public void ObjectFlagSetState(bool state)
    // {
    //     if (state)
    //     {
    //         gateAnim.SetTrigger("interaction");
    //     }
    // }
}
