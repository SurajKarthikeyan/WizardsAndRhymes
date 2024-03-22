using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class EventOnTrigger : MonoBehaviour
{
    [Tooltip("The events to trigger when the player enters the collider")]
    [SerializeField] UnityEvent onTrigger;
    [Tooltip("The list of tags that can trigger this trigger")]
    [SerializeField] string[] triggerTags = { "Player" };

    private void OnTriggerEnter(Collider other)
    {
        //Trigger the event if an object with a valid tag entered the trigger
        foreach (string tag in triggerTags)
        {
            if (other.CompareTag(tag))
            {
                onTrigger?.Invoke();
                return;
            }
        }
    }
}
