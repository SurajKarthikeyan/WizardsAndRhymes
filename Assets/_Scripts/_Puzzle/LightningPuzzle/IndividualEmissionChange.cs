using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndividualEmissionChange : MonoBehaviour
{
    #region Vars

    [Tooltip("Object to change emmision material")]
    [SerializeField] private GameObject go;
    [Tooltip("Emission Material to copy")]
    [SerializeField] private Material originalMaterial;
    [Tooltip("Copy of emission material, do not set")]
    [SerializeField] private Material newMaterial;
    [Tooltip("Time to reach max or minimum emission")]
    [SerializeField] private float timeToFullEmission;

    [Tooltip("To check if to turn this object emmisive or not")]
    [SerializeField] public bool isOn;
    
    [ColorUsage(true, true)]
    [SerializeField] private Color minEmissionValue;
    [ColorUsage(true, true)]
    [SerializeField] private Color maxEmissionValue;

    #endregion


    #region UnityMethods

    private void Awake()
    {
        newMaterial = new Material(originalMaterial);
        newMaterial.EnableKeyword("_EMISSION");
        go.GetComponent<Renderer>().material = newMaterial;
        
    }

    #endregion
    
    #region CustomMethods

    /// <summary>
    /// One direction emission cycle
    /// </summary>
    public void AlterEmissionCycle()
    {
        StartCoroutine(AlterEmissionFullCycle());
    }

    public IEnumerator AlterEmissionFullCycle()
    {
        float timer = 0;
        while (timer < timeToFullEmission * 2)
        {
            Color color = newMaterial.GetColor("_EmissionColor");
            if (timer < timeToFullEmission)
            {
                color = Color.Lerp(color, maxEmissionValue, timer/timeToFullEmission);
            }

            else
            {
                color = Color.Lerp(color, minEmissionValue, (timer - timeToFullEmission)/timeToFullEmission);
            }
            newMaterial.SetColor("_EmissionColor", color);
            go.GetComponent<Renderer>().material = newMaterial;
            timer += Time.deltaTime;
            yield return null;
        }
    }

    /// <summary>
    /// Up and down emission cycle
    /// </summary>
    /// <param name="up"></param>
    /// <returns></returns>
    public IEnumerator AlterEmissionOverTime(bool up)
    {
        float timer = 0;
        Color color = newMaterial.GetColor("_EmissionColor");

        if (up)
        {
            while(timer < timeToFullEmission)
            {
                color = Vector4.Lerp(color, maxEmissionValue, timer / timeToFullEmission);
                newMaterial.SetColor("_EmissionColor", color);
                go.GetComponent<Renderer>().material = newMaterial;
                timer += Time.deltaTime;
                yield return new WaitForSeconds(Time.deltaTime);

            }
        }

        else
        {
            while(timer < timeToFullEmission)
            {
                color = Vector4.Lerp(color, minEmissionValue, timer / timeToFullEmission);

                newMaterial.SetColor("_EmissionColor", color);
                go.GetComponent<Renderer>().material = newMaterial;
                timer += Time.deltaTime;
                yield return new WaitForSeconds(Time.deltaTime);

            }
        }

        newMaterial.SetColor("_EmissionColor", color);
        go.GetComponent<Renderer>().material = newMaterial;
        yield return null;
        }
    }

    #endregion
