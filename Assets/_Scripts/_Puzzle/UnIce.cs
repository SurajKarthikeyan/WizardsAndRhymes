using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnIce : MonoBehaviour
{
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController.instance.isOnIce = false;
        }
    }
}
