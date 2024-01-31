using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Enemy type that follows the player
/// </summary>
public class FollowPlayerEnemy : BaseEnemy
{
    NavMeshAgent navMeshAgent;

    public Transform player;

    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (hp > 0)
        {
            navMeshAgent.destination = player.position;
        }
    }
}
