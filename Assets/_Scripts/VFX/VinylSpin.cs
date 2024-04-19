using System;
using System.Collections;
using System.Collections.Generic;
using Dreamteck.Splines.Primitives;
using UnityEngine;

public class VinylSpin : MonoBehaviour
{
    #region Vars

    [SerializeField] private Floater discFloater;
    [SerializeField] private int maxDegreesPerSecond;
    [SerializeField] private float timeToSpinAtMaxSpeed;
    [HideInInspector] private float minDegreesPerSecond;

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
        StartCoroutine(SpinDiscCoroutine());
    }

    public IEnumerator SpinDiscCoroutine()
    {
        discFloater.degreesPerSecond = maxDegreesPerSecond;
        yield return new WaitForSeconds(timeToSpinAtMaxSpeed);
        discFloater.degreesPerSecond = minDegreesPerSecond;
    }

    #endregion

}
