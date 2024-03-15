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


    // Start is called before the first frame update
    void Start()
    {
        transform.position = startPos.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (activated)
        {
            if (movingToEnd)
            {
                transform.position = Vector3.MoveTowards(transform.position, endPos.position, movementSpeed * Time.deltaTime);
                if (Vector3.Distance(transform.position, endPos.position) <= 0.1f)
                {
                    movingToEnd = false;
                }
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, startPos.position, movementSpeed * Time.deltaTime);
                if (Vector3.Distance(transform.position, startPos.position) <= 0.1f)
                {
                    movingToEnd = true;
                }
            }
        }    
    }

    public void Activate()
    {
        activated = true;
    }
    
}
