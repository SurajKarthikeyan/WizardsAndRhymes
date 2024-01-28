using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using UnityEngine.Experimental.VFX;
using UnityEngine;
using UnityEngine.VFX;

public class FireVFXPulseTest : MonoBehaviour
{


    [SerializeField] private VisualEffect fireTestVFX;
    private float floor;
    private float ceiling;
    private float pulsePerc;
    public float scaleUp;
    public float scaleDown;

    private void Start()
    {
        floor = 3;
        ceiling = 5;

    }

    public void FirePulse()
    {
        StartCoroutine(ChangeValueLarge(floor, ceiling, scaleUp));
    }

    IEnumerator ChangeValueLarge(float start, float end, float duration)
    {
        float elapsed = 0.0f;
        while (elapsed < duration )
        {
            pulsePerc = Mathf.Lerp( start, end, elapsed / duration );
            fireTestVFX.SetFloat("flameSizeDelta", pulsePerc);
            elapsed += Time.deltaTime;
            yield return null;
        }

        pulsePerc = end;
        StartCoroutine(ChangeValueSmall(ceiling, floor, scaleDown));
    }
    
    IEnumerator ChangeValueSmall(float start, float end, float duration)
    {
        float elapsed = 0.0f;
        while (elapsed < duration )
        {
            pulsePerc = Mathf.Lerp( start, end, elapsed / duration );
            fireTestVFX.SetFloat("flameSizeDelta", pulsePerc);
            elapsed += Time.deltaTime;
            yield return null;
        }

        pulsePerc = end;
    }
}
