using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Script to describe individual lever, tasked with flipping itself and the light as well as notifying LeverManager
/// </summary>
public class Lever : MonoBehaviour
{
    #region Vars
    [Tooltip("Reference to leverManager for this lever")]
    [SerializeField] private LeverManager leverManager;
    [Tooltip("Light associated with this lever")]
    [SerializeField] private Light spotLight;
    [Tooltip("Off Color for spotlight")]
    [SerializeField] private Color offColor;
    [Tooltip("On Color for spotlight")]
    [SerializeField] private Color onColor;
    [Tooltip("Wwise sound event")]
    [SerializeField] private AK.Wwise.Event leverSoundEvent;
    [Tooltip("Bool if the lever is on")]
    [SerializeField] public bool isOn;
    [Tooltip("Electric block to turn on when lever is turned on")]
    [SerializeField] private GameObject electricBlock;
    [Tooltip("Animator for lever")]
    [SerializeField] private Animator leverAnimator;
    [Tooltip("List of other levers to affect")]
    [SerializeField] private List<Lever> otherLevers;
    #endregion


    #region UnityMethods

    private void Start()
    {
        ChangeColorLight(isOn);
        if (!isOn)
        {
            leverAnimator.SetTrigger("interaction");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !leverManager.isLeverCoolDown && !leverManager.completedLeverSystem)
        {
            LeverSwitch(true, true);
        }
    }

    #endregion

    #region CustomMethods

    private void ChangeColorLight(bool lightStatus)
    {
        if (lightStatus)
        {
            spotLight.color = onColor;
        }
        else
        {
            spotLight.color = offColor; 
        }
    }

    public void LeverSwitch(bool sound, bool listSwitch)
    {
        isOn = !isOn;
        leverAnimator.SetTrigger("interaction");
        if (electricBlock)
        {
            electricBlock.SetActive(isOn);
        }
        ChangeColorLight(isOn);
        if (sound)
        {
            leverSoundEvent.Post(this.gameObject);
        }

        if (listSwitch)
        {
            OtherLeverSwitch();
        }
        leverManager.CheckLevers();

    }

    public void OtherLeverSwitch()
    {
        for (int i = 0; i < otherLevers.Count; i++)
        {
            otherLevers[i].LeverSwitch(false, false);
        }
    }

    #endregion

}
