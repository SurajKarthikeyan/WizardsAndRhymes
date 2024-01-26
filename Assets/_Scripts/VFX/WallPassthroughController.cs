using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallPassthroughController : MonoBehaviour
{

    [Tooltip("The player's transform")]
    [SerializeField] Transform playerTransform;
    [Tooltip("The wall material")]
    [SerializeField] Material wallMaterial;
    [Tooltip("The layers to raycast against when detecting walls")]
    [SerializeField] LayerMask wallLayerMask;

    private void Update()
    {
        Vector3 rayOrigin = Camera.main.transform.position + Camera.main.nearClipPlane * Camera.main.transform.forward;
        float playerDistance = Vector3.Distance(rayOrigin, playerTransform.position);
        if (Physics.Raycast(rayOrigin, Camera.main.transform.forward, playerDistance, wallLayerMask))
        {
            //There is a wall in between the player and the camera

        }
    }
}
