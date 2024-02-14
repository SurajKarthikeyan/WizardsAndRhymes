using UnityEngine;

/// <summary>
/// Enemy type that follows the player and strafes around them before attacking
/// </summary>
public class MeleeStrafeEnemy : BaseEnemyBehavior
{
    #region Variables
    public enum BehaviorState
    {
        Inactive,
        TrackingPlayer,
        Strafing,
        Attacking
    }

    public BehaviorState behaviorState;

    [Tooltip("Transform of the player to follow")]
    [SerializeField]
    private Transform player;

    [Tooltip("Maximum distance this enemy will be from the player before it shoots")]
    [SerializeField]
    private float maxDistance = 10f;

    [Tooltip("Minimum distance this enemy will be from the player before it shoots")]
    [SerializeField]
    private float minDistance = 1f;

    public float strafeSpeed;

    public float strafeTimeThreshold;

    public float strafeTime;

    public float meleeLungeDistance = 5f;

    public Rigidbody rb;
    #endregion


    #region Unity Methods

    protected override void Start()
    {
        base.Start();
        if (strafeSpeed <= 0)
        {
            strafeSpeed = navMeshAgent.speed;
        }
    }
    /// <summary>
    /// Unity method called every frame update
    /// </summary>
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (activated)
        {
            if (!rb.isKinematic)
            {
                if (rb.velocity.magnitude <= 0.5f)
                {
                    navMeshAgent.enabled = true;
                    //navMeshAgent.updateRotation = true;
                    //rb.constraints = RigidbodyConstraints.None;
                    rb.isKinematic = true;
                    behaviorState = BehaviorState.TrackingPlayer;
                }
            }
            else
            {
                float currDistance = Vector3.Distance(transform.position, player.transform.position);
                if (currDistance > maxDistance)
                {
                    behaviorState = BehaviorState.TrackingPlayer;
                    navMeshAgent.destination = player.position;
                }
                //else if (currDistance < minDistance)
                //{
                //    Retreat();
                //}
                else if (currDistance < maxDistance && currDistance > minDistance)
                {

                    if (behaviorState == BehaviorState.TrackingPlayer || behaviorState == BehaviorState.Strafing)
                    {
                        if (strafeTime >= strafeTimeThreshold)
                        {
                            behaviorState = BehaviorState.Attacking;
                            strafeTime = 0;
                        }
                        else
                        {
                            //int strafeRightNum = Random.Range(0, 1);

                            //bool strafeRight = strafeRightNum != 0;

                            bool strafeRight = true;

                            Strafe(strafeRight);

                            strafeTime += Time.deltaTime;
                        }

                    }
                    else if (behaviorState == BehaviorState.Attacking)
                    {
                        if (Vector3.Distance(transform.position, player.transform.position) >= meleeLungeDistance)
                        {
                            LungeAttack();
                        }
                        else
                        {
                            navMeshAgent.updateRotation = false;
                            Retreat();
                        }                        
                    }

                }
            }
            
        }
    }
    #endregion

    #region Custom Methods
    public void Strafe(bool strafeRight = true)
    {
        Vector3 offsetPlayer;
        if (strafeRight)
        {
            offsetPlayer = player.position - transform.position;
        }
        else
        {
            offsetPlayer = transform.position - player.position;
        }
        Vector3 dir = Vector3.Cross(offsetPlayer, Vector3.up);
        navMeshAgent.SetDestination(transform.position + dir);
        Vector3 lookPos = player.transform.position - transform.position;
        lookPos.y = 0;
        Quaternion rotation = Quaternion.LookRotation(lookPos, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 15);
    }

    public void Retreat()
    {
        Vector3 dirToPlayer = transform.position - player.transform.position;
        Vector3 runPos = transform.position + dirToPlayer;
        navMeshAgent.SetDestination(runPos);
    }

    public void LungeAttack()
    {
        //navMeshAgent.updateRotation = false;
        navMeshAgent.enabled = false;
        rb.isKinematic = false;
        //rb.constraints = RigidbodyConstraints.FreezeRotationZ;
        //rb.constraints = RigidbodyConstraints.FreezeRotationX;
        rb.AddForce((player.transform.position - transform.position) * 1.65f, ForceMode.Impulse);
    }
    #endregion
}
