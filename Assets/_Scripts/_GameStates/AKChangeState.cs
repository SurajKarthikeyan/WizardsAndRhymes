using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AKChangeState : MonoBehaviour
{
    public AK.Wwise.State akState;
    [SerializeField] private AK.Wwise.Event hauntedStinger;
    [SerializeField] private bool useHauntedStinger;

    public void ChangeState()
    {
        akState.SetValue();
        if (useHauntedStinger)
        {
            hauntedStinger.Post(this.gameObject);
        }
    }
}
