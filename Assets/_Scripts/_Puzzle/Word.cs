using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Word : MonoBehaviour
{
    #region Variables
    [SerializeField] private List<string> flagsToSet = new();
    [SerializeField] private AK.Wwise.Event wordPickUpSoundEffect;
    [SerializeField] private GameObject decal;
    [Tooltip("If not set, aims target to MainAreaInstance singleton gameobject")]
    [SerializeField] private GameObject postWordTarget;
    public AKChangeState iceOutOfPuzzleState;
    #endregion

    #region UnityMethods

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (postWordTarget != null)
            {
                SetPointerTarget(postWordTarget);

            }
            else
            {
                ObjectivePointer.instance.gameObject.SetActive(true);
                SetPointerTarget(MainAreaInstance.instance.gameObject);
            }
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
            iceOutOfPuzzleState.ChangeState();

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
