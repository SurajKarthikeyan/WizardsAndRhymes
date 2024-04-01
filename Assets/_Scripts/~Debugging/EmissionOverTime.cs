using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EmissionOverTime : MonoBehaviour
{
    public Material mat;
    public Renderer rend;
    public Material newMat;
    private void Start()
    {
        newMat = new Material(mat);
        rend.material = newMat;
        newMat.EnableKeyword("_EMISSION");
        Color color = newMat.GetColor("_EmissionColor");
        color *= 40;
        newMat.SetColor("_EmissionColor", color);
        rend.material = newMat;
        //StartCoroutine(AlterEmissionOverTime());
    }

    IEnumerator AlterEmissionOverTime()
    {
        for (int i = 0; i < 10; i++)
        {
            Color color = newMat.GetColor("_EmissionColor");
            color *= 1.1f;
            newMat.SetColor("_EmissionColor", color);
            rend.material = newMat;
            yield return new WaitForSeconds(0.05f);
        }
    }
}
