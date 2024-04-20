using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Word : MonoBehaviour
{
    #region Variables
    [Tooltip("Flags to set in the flag manager")]
    [SerializeField] private List<string> flagsToSet = new();
    [Tooltip("Wwise Sound Effect")]
    [SerializeField] private AK.Wwise.Event wordPickUpSoundEffect;
    [Tooltip("Decal to spawn when word is picked up")]
    [SerializeField] private GameObject decal;
    [Tooltip("If not set, aims target to MainAreaInstance singleton gameobject")]
    [SerializeField] private GameObject postWordTarget;
    public AKChangeState iceOutOfPuzzleState;
    [Tooltip("VFX Object to instantiate when cassette is picked up")]
    [SerializeField] private GameObject cassettePickUpEffect;
    #endregion

    #region UnityMethods

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            
            GameObject vfxInstance =  Instantiate(cassettePickUpEffect);
            vfxInstance.transform.position = this.transform.position;
            if (postWordTarget != null)
            {
                SetPointerTarget(postWordTarget);

            }
            /*else
            {
                ObjectivePointer.instance.gameObject.SetActive(true);
                SetPointerTarget(MainAreaInstance.instance.gameObject);
            }*/
            decal.SetActive(true);
            wordPickUpSoundEffect.Post(other.gameObject);
            foreach(string flag in flagsToSet)
            {
                FlagManager.instance.SetFlag(flag, true);
            }
            
            // puzzleToggle.isOn = true;
            if (FlagManager.instance.HasAllWordFlags())
            {
                //set rap rock to interactable
                Debug.Log("Rap rock interactable");
            }

            if (iceOutOfPuzzleState != null)
            {
                iceOutOfPuzzleState.ChangeState();
            }

            Destroy(gameObject);
        }
    }

    #endregion


    #region CustomMethods

    private void SetPointerTarget(GameObject go)
    {
        ObjectivePointer.instance.SetTargetGameObject(go);
    }

    #endregion
}
