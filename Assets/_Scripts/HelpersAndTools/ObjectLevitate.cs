using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectLevitate : MonoBehaviour
{
    public float speed = 1f; // Speed of levitation
    public float distance = 1f; // Distance between up and down points
    public bool randomize = false; // Option to randomize speed and distance
    public float minSpeed = 0.5f; // Minimum speed when randomizing
    public float maxSpeed = 2f; // Maximum speed when randomizing
    public float minDistance = 0.5f; // Minimum distance when randomizing
    public float maxDistance = 2f; // Maximum distance when randomizing

    private Vector3 startPos;
    private float originalSpeed;
    private float originalDistance;

    void Start()
    {
        startPos = transform.position; // Store the initial position of the object
        originalSpeed = speed;
        originalDistance = distance;

        if (randomize)
        {
            RandomizeValues();
        }
    }

    void Update()
    {
        // Calculate the new Y position using a sine wave to create the levitating effect
        float newY = startPos.y + Mathf.Sin(Time.time * speed) * distance;

        // Update the object's position
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }

    void RandomizeValues()
    {
        speed = Random.Range(minSpeed, maxSpeed);
        distance = Random.Range(minDistance, maxDistance);
    }

    void OnValidate()
    {
        if (randomize)
        {
            RandomizeValues();
        }
        else
        {
            speed = originalSpeed;
            distance = originalDistance;
        }
    }
}
