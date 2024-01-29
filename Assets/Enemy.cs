using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{

    NavMeshAgent navMeshAgent;

    public Transform player;

    public int hp;
    // Start is called before the first frame update


    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        navMeshAgent.destination = player.position;

        if (hp <= 0)
        {
            Destroy(gameObject);
        }
    }
}
