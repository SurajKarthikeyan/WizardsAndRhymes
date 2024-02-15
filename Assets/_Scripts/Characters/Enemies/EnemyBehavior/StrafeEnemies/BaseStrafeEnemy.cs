using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Parent class of enemies who perform strafing actions
/// </summary>
public abstract class BaseStrafeEnemy : BaseEnemyBehavior
{
    [Header("Strafing variables")]
    [Tooltip("Speed that this enemy strafes, if set to 0 it will take the speed of the navMesh agent")]
    [SerializeField]
    protected float strafeSpeed;

    [Tooltip("Time in seconds in which this enemy will strafe for")]
    [SerializeField]
    protected float strafeTimeThreshold;

    [Tooltip("Boolean determining whether or not we strafe right or not")]
    protected bool strafeRight;

    [Tooltip("Current time elapsed in which this enemy is strafing")]
    protected float strafeTime;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        //Sets the default strafing speed to be the speed of the navMesh
        if (strafeSpeed <= 0)
        {
            strafeSpeed = navMeshAgent.speed;
        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (activated)
        {
            EvaluateBehavior();
            LookAtPlayer();
        }
    }

    /// <summary>
    /// Function that lets this enemy strafe
    /// </summary>
    /// <param name="strafeRight"></param>
    protected void Strafe(bool strafeRight = true)
    {
        Vector3 offsetPlayer;
        /**
         * Conditional calculates different vector direction, magnitude remains same
         */
        if (strafeRight)
        {
            offsetPlayer = PlayerController.instance.transform.position - transform.position;
        }
        else
        {
            offsetPlayer = transform.position - PlayerController.instance.transform.position;
        }
        /**
         * Cross product will calculate vector perpendicular to both the vector between the player
         * and the upwards vector. 
         * Result will be a vector pointing left or right relative to the direction facing the player
         * */
        Vector3 dir = Vector3.Cross(offsetPlayer, Vector3.up);
        //Move to that location that we just found
        navMeshAgent.SetDestination(transform.position + dir);
        LookAtPlayer();
    }

    protected override void EvaluateBehavior()
    {
        if (!rb.isKinematic)
        {
            /**
             * Check is here because navMesh and Unity physics do not play nice with each other
             * This is when the enemy is currently lunging at the player
             */
            if (rb.velocity.magnitude <= 0.5f)
            {
                //All this is to get the navMesh back following the player
                navMeshAgent.enabled = true;
                rb.isKinematic = true;
                behaviorState = EnemyBehaviorState.TrackingPlayer;
            }
        }
        //Else statement is when the enemy is not lunging at the player
        else
        {
            EvaluateDistanceFromPlayer();
        }
    }

    protected override void EvaluateDistanceFromPlayer()
    {
        float currDistance = Vector3.Distance(transform.position, PlayerController.instance.transform.position);
        //If the enemy is too far from the player, it will continue to follow it
        if (currDistance > maxDistance)
        {
            behaviorState = EnemyBehaviorState.TrackingPlayer;
            navMeshAgent.destination = PlayerController.instance.transform.position;
        }
        //If the enemy is too close to the player
        else if (currDistance < minDistance)
        {
            SmallRetreat();
        }
        //If the enemy is in between the max and the min
        else
        {
            SpecializedBehavior();
        }
    }
}
