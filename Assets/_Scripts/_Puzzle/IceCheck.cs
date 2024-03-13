using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceCheck : MonoBehaviour
{
    #region UnityMethods
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController.instance.isOnIce = true;
            PlayerController.instance.SaveCurrentVelocityVector();
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController.instance.isOnIce = false;
        }
    }

    #endregion
}
