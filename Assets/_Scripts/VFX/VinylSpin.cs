using System;
using System.Collections;
using System.Collections.Generic;
using Dreamteck.Splines.Primitives;
using Language.Lua;
using UnityEngine;

public class VinylSpin : MonoBehaviour
{
    #region Vars

    [SerializeField] private Floater discFloater;
    [SerializeField] private int maxDegreesPerSecond;
    [SerializeField] private float timeToSpinAtMaxSpeed;
    [SerializeField] private float timeToReachMaxSpeed;
    [HideInInspector] private float minDegreesPerSecond;
    [SerializeField] private AK.Wwise.Event recordScratch;
    [SerializeField] private Material pressedMat;
    [SerializeField] private Material normalMat;
    [SerializeField] private Renderer discRenderer;
    private IEnumerator spinDiscInstance;

    #endregion

    #region UnityMethods

    private void Start()
    {
        minDegreesPerSecond = discFloater.degreesPerSecond;
    }

    #endregion

    #region CustomMethods
    
    public void SpinDisc()
    {
        if (spinDiscInstance == null)
        {
            spinDiscInstance = SpinDiscCoroutine();
            recordScratch.Post(this.gameObject);
            StartCoroutine(spinDiscInstance);
        }
    }

    public IEnumerator SpinDiscCoroutine()
    {
        discRenderer.material = pressedMat;
        float timer = 0f;
        float newSpeed = minDegreesPerSecond;
        while (timer < timeToReachMaxSpeed)
        {
            newSpeed = Mathf.Lerp(newSpeed, maxDegreesPerSecond, timer / timeToReachMaxSpeed);
            discFloater.degreesPerSecond = newSpeed;
            timer += Time.deltaTime;
        }
        
        
        yield return new WaitForSeconds(timeToSpinAtMaxSpeed);
        
        timer = 0f;
        newSpeed = maxDegreesPerSecond;
        while (timer < timeToReachMaxSpeed)
        {
            newSpeed = Mathf.Lerp(newSpeed, minDegreesPerSecond, timer / timeToReachMaxSpeed);
            discFloater.degreesPerSecond = newSpeed;
            timer += Time.deltaTime;
        }
        discRenderer.material = normalMat;
        spinDiscInstance = null;
    }

    #endregion

}
