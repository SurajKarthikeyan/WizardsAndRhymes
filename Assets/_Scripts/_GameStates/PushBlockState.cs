using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushBlockState : MonoBehaviour
{
    #region Vars

    [SerializeField] private AK.Wwise.Event metalPushBlockEffect;

    #endregion

    #region UnityMethods

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            metalPushBlockEffect.Post(this.gameObject);
            AkSoundEngine.SetState("MetalBlockPush", "IsPushing");
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            AkSoundEngine.SetState("MetalBlockPush", "NotPushing");
        }
    }

    #endregion
}
