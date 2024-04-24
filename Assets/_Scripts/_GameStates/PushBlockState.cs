using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushBlockState : MonoBehaviour
{
    #region Vars

    [SerializeField] private AK.Wwise.Event metalPushBlockEffect;
    [SerializeField] private AK.Wwise.State setPushBlock;
    [SerializeField] private AK.Wwise.State setNotPushBlock;

    #endregion

    #region UnityMethods

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            metalPushBlockEffect.Post(this.gameObject);
            setPushBlock.SetValue();
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            setNotPushBlock.SetValue();
        }
    }

    #endregion
}
