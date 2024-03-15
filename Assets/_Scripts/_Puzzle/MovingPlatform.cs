using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{

    public Transform startPos;

    public Transform endPos;

    public bool activated;

    public bool movingToEnd = true;

    public float movementSpeed = 5f;

    public bool ableToMove = true;

    public Vector3 destinationPos;

    public bool AtDestination => Vector3.Distance(transform.position, destinationPos) <= 0.1f;

    public bool stopAtDestination = false;


    // Start is called before the first frame update
    void Start()
    {
        transform.position = startPos.position;
        destinationPos = endPos.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (activated && ableToMove)
        {
            if (movingToEnd)
            {
                transform.position = Vector3.MoveTowards(transform.position, endPos.position, movementSpeed * Time.deltaTime);
                if (AtDestination)
                {
                    movingToEnd = false;
                    destinationPos = startPos.position;
                    if (stopAtDestination)
                    {
                        ableToMove = false;
                    }
                    else
                    {
                        ableToMove = true;
                    }
                }
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, startPos.position, movementSpeed * Time.deltaTime);
                if (AtDestination)
                {
                    movingToEnd = true;
                    destinationPos = endPos.position;
                    if (stopAtDestination)
                    {
                        ableToMove = false;
                    }
                    else
                    {
                        ableToMove = true;
                    }
                }
            }
        }    
    }

    public void Activate()
    {
        activated = true;
    }

    public void SendToStart()
    {
        movingToEnd = false;
        destinationPos = startPos.position;
        ableToMove = true;
    }
    
}
