using System;
using System.Collections;
using System.Collections.Generic;
using Dreamteck.Splines.Primitives;
using UnityEngine;

/// <summary>
/// Class describing lever
/// </summary>
public class Lever : MonoBehaviour, IInteractable
{

    #region Variables

    [Tooltip("Checks if the lever is On")]
    [SerializeField] private bool isOn;

    [Tooltip("Lever Animator")]
    [SerializeField] private Animator leverAnimator;

    [Tooltip("Wwise event for lever sfx")]
    [SerializeField] private AK.Wwise.Event leverWwiseEvent;

    [Tooltip("Reference to animator interaction trigger")]
    private static readonly int Interaction = Animator.StringToHash("interaction");

    [SerializeField] private List<GameLight> lightList;

    #endregion

    #region UnityMethods

    private void Start()
    {
        isOn = false;
    }

    #endregion

    #region CustomMethods

    private void FlipLights()
    {
        for (int i = 0; i < lightList.Count; i++)
        {
            lightList[i].FlipLight();
        }
    }
    

    #endregion

    #region InterfaceMethods
    public void Interact()
    {
        leverWwiseEvent.Post(this.gameObject);
        leverAnimator.SetTrigger(Interaction);
        isOn = !isOn;
        FlipLights();
    }

    #endregion
}
