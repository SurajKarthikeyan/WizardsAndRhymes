using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AKChangeState : MonoBehaviour
{
    public AK.Wwise.State akState;
    [SerializeField] private AK.Wwise.Event hauntedStinger;
    [SerializeField] private bool useHauntedStinger;
    [SerializeField] private bool destroyOnCollision;

    public void ChangeState()
    {
        akState.SetValue();
        if (useHauntedStinger)
        {
            hauntedStinger.Post(this.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (destroyOnCollision && other.gameObject.CompareTag("Player"))
        {
            akState.SetValue();
            Destroy(this.gameObject);
        }
    }

}
