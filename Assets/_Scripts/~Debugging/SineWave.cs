using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SineWave : MonoBehaviour
{
    public LineRenderer myLineRenderer;
    public int points;
    public float amplitude = 1;
    public float frequency = 1;
    public Vector2 xLimit = new Vector2(0,1);
    public float movementSpeed = 1;
    void Start()
    {
        myLineRenderer = this.GetComponent<LineRenderer>();
        
    }

    private void Update()
    {
        Draw();
    }

    public void Draw()
    {
        float xStart = xLimit.x;
        float Tau = 2 * Mathf.PI;
        float xFinish = xLimit.y;

        myLineRenderer.positionCount = points;

        for (int currentPoint = 0; currentPoint < points; currentPoint++)
        {
            float progress = (float)currentPoint / (points - 1);
            float x = Mathf.Lerp(xStart, xFinish, progress);

            float y = amplitude * Mathf.Sin((Tau * frequency * x) + (Time.timeSinceLevelLoad * movementSpeed));
            myLineRenderer.SetPosition(currentPoint, new Vector3(x,y,0));
        }
    }
    
}
