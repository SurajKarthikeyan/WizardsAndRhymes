using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A base sigleton class other MonoBehaviours can inherit from
/// </summary>
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    [Tooltip("Singleton instance")]
    public static T instance;

    /// <summary>
    /// Initialize singleton instance
    /// </summary>
    protected virtual void Awake()
    {
        if (instance == null)
        {
            //Find the inherited component of this instance
            T[] componentInstances = GetComponents<T>();
            foreach (T component in componentInstances)
            {
                if (component == this)
                {
                    instance = component;
                    break;
                }
            }
        }
        else
        {
            Debug.LogWarning("Duplicate " + nameof(T) + " attached to " + gameObject.name + ". Destroying.");
            Destroy(this);
        }
    }

    /// <summary>
    /// Free singleton instance
    /// </summary>
    protected virtual void OnDestroy()
    {
        if (instance == this)
            instance = null;
    }
}
