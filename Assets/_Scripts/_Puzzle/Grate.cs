using System.Collections;
using System.Collections.Generic;
using AK.Wwise;
using UnityEngine;

public class Grate : Gate
{
    public GameObject grate;
    public override void SwitchGate()
    {
        grate.SetActive(false);
    }
}
