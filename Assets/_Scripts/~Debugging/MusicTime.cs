using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicTime : MonoBehaviour
{
    [SerializeField] GameObject cube;

    public void Sync()
    {
        cube.SetActive(!cube.activeSelf);
    }
}
