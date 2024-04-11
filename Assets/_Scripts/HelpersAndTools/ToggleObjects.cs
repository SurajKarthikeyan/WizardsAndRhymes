using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleObjects : MonoBehaviour
{
    public List<GameObject> objects = new List<GameObject>();

    public void ToggleAll()
    {
        foreach (GameObject listObject in objects)
        {
            listObject.SetActive(!listObject.activeInHierarchy);
        }
    }
    
    public void ToggleAllColliders()
    {
        foreach (GameObject listObject in objects)
        {
            if (listObject.TryGetComponent(out Collider objectCollider))
            {
                objectCollider.enabled = !objectCollider.enabled;
            }
        }
    }
        
}
