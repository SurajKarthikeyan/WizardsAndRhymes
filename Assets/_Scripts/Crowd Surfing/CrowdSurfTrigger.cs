using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdSurfTrigger : MonoBehaviour
{
    [Tooltip("The crowd surf path triggered by this component")]
    [SerializeField] CrowdSurfPath crowdSurfPath;
    [Tooltip("Whether this trigger is at the end of the path instead of the start")]
    [SerializeField] bool end;

    bool ignorePlayerInCollider;

    private void Start()
    {
        Initialize();
    }

    [ContextMenu("Initialize")]
    void Initialize()
    {
        if (end)
        {
            if (!crowdSurfPath.IsTwoWay())
            {
                gameObject.SetActive(false);
                return;
            }
            transform.position = crowdSurfPath.GetPositionOnPath(1);
        }
        else
            transform.position = crowdSurfPath.GetPositionOnPath(0);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Entered trigger: " + gameObject.name);

        if (crowdSurfPath.PlayerOnPath || ignorePlayerInCollider) //Don't try to start a crowd-surf if the player is already crowd surfing
        {
            ignorePlayerInCollider = true;
            return;
        }

        if (other.CompareTag(PlayerController.PlayerTag))
        {
            crowdSurfPath.StartCrowdSurf(end);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        ignorePlayerInCollider = false;
    }
}
