using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AKChangeState : MonoBehaviour
{
    public AK.Wwise.State akState;

    public void ChangeState()
    {
        akState.SetValue();
    }
}
