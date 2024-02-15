using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseStrafeEnemy : BaseEnemyBehavior
{
    [Header("Strafing variables")]
    [Tooltip("Speed that this enemy strafes, if set to 0 it will take the speed of the navMesh agent")]
    [SerializeField]
    protected float strafeSpeed;

    [Tooltip("Time in seconds in which this enemy will strafe for")]
    [SerializeField]
    protected float strafeTimeThreshold;
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

    //// Update is called once per frame
    //void Update()
    //{

    //}

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
}
