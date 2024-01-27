using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicTime : MonoBehaviour
{
    public float bpm = 135;
    public float nextBeatTime;

    public AudioSource source;

    // Start is called before the first frame update
    /*void Start()
    {
        source.Play();
        CalculateNextBeat();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= nextBeatTime)
        {
            CalculateNextBeat();
            Debug.Log(nextBeatTime);
        }
    }

    public void CalculateNextBeat()
    {
        nextBeatTime = Time.time + (1 / (bpm / 60));
        //Debug.Log(1 / (bpm / 60));
    }*/

    public void Sync()
    {
        Debug.Log("Hello");
    }
}
