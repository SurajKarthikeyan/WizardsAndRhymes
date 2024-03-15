using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour, IInteractable
{
    #region Variables

    [Tooltip("Checks if the lever is On")]
    [SerializeField] private bool isOn;

    [Tooltip("Lever Animator")]
    [SerializeField] private Animator pressurePlateAnimator;

    [Tooltip("Wwise event for lever sfx")]
    [SerializeField] private AK.Wwise.Event pressurePlateWWiseEvent;

    [Tooltip("Reference to animator interaction trigger")]
    private static readonly int Interaction = Animator.StringToHash("interaction");


    #endregion

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Interact()
    {
        Debug.Log("Interacting with pressurePlate)");
        isOn = !isOn;
        pressurePlateAnimator.SetTrigger(Interaction);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isOn)
        {
            if (other.gameObject.TryGetComponent(out PushBox pushBox) ||
            other.gameObject.TryGetComponent(out PlayerController player))
            {
                Debug.Log(other.gameObject.name + "entering");
                Interact();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (isOn)
        {
            if (other.gameObject.TryGetComponent(out PushBox pushBox) ||
            other.gameObject.TryGetComponent(out PlayerController player))
            {
                Debug.Log(other.gameObject.name + "leaving");
                Interact();
            }
        }
    }
}
