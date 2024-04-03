using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    #region Vars
    [Tooltip("Pressure Plate SFX")]
    [SerializeField] private AK.Wwise.Event pressurePlateSoundEffect;

    [Tooltip("If this is not a pushable electroblock, set the GO for it to activate")]
    [SerializeField] private GameObject activatableGameObject;

    [Tooltip("Aniamtor for pressure plate")]
    [SerializeField] private Animator pressurePlateAnimator;
    
    [Tooltip("Bool for if its been interacted with and reverse the animation on exit")]
    [SerializeField] private bool hasBeenPushedDown;


    private bool isElectroBlock;
    #endregion

    #region UnityMethods

    private void OnTriggerEnter(Collider other)
    {
        pressurePlateSoundEffect.Stop(this.GameObject());
        pressurePlateSoundEffect.Post(this.GameObject());
        if (other.CompareTag("PushBox") || (other.CompareTag("Player")))
        {
            ActivateDefaultPressurePlate();
            hasBeenPushedDown = true;
            pressurePlateAnimator.SetTrigger("interaction");
            isElectroBlock = false;
        }
        else if (other.CompareTag("PushBoxEletric"))
        {
            isElectroBlock = true;
            ActivateElectroBlockPressurePlate(other);
            hasBeenPushedDown = true;
            pressurePlateAnimator.SetTrigger("interaction");
        }

        
    }

    private void OnTriggerExit(Collider other)
    {
        pressurePlateSoundEffect.Stop(this.GameObject());
        pressurePlateSoundEffect.Post(this.GameObject());
        if (hasBeenPushedDown)
        {
            hasBeenPushedDown = false;
            pressurePlateAnimator.SetTrigger("interaction");

            if (isElectroBlock)
            {
                other.GameObject().GetComponent<FirstLightningBlock>().isActive = false;
            }

            else
            {
                activatableGameObject.SetActive(!activatableGameObject.activeSelf);
            }
        }
    }

    /// <summary>
    /// Default interaction with pressure plate, disables/enables appropriate GO
    /// </summary>
    public void ActivateDefaultPressurePlate()
    {
        activatableGameObject.SetActive(!activatableGameObject.activeSelf);
    }

    /// <summary>
    /// Pushable Electroblock function
    /// </summary>
    public void ActivateElectroBlockPressurePlate(Collider electroBlock)

    {
        electroBlock.GameObject().GetComponent<FirstLightningBlock>().isActive = true;
    }

    #endregion
}
