using UnityEngine;

/// <summary>
/// Enemy type that follows the player and strafes around them before attacking
/// </summary>
public class MeleeStrafeEnemy : BaseStrafeEnemy
{
    #region Variables
    [Header("Melee specific attack variables")]
    [Tooltip("Distance in which the enemy will lunge at the player")]
    [SerializeField]
    private float meleeLungeDistance = 5f;

    [Tooltip("Force in which the enemy will lunge at the player with")]
    [SerializeField]
    private float meleeLungeForce = 10f;
    #endregion

    #region Custom Methods
    /// <summary>
    /// Function that makes this enemy lunge at the player
    /// </summary>
    private void LungeAttack()
    {
        //BEGIN MELEE ANIMATION
        //Temporarily disable the navMesh because Unity physics do not like it
        navMeshAgent.enabled = false;
        rb.isKinematic = false;
        //Add a force in the direction the enemy is facing
        rb.AddForce((PlayerController.instance.transform.position - transform.position).normalized * meleeLungeForce, ForceMode.Impulse);
    }

    protected override void SpecializedBehavior()
    {
        //If the enemy is either coming into range from tracking the player or is already strafing
        if (behaviorState == EnemyBehaviorState.TrackingPlayer || behaviorState == EnemyBehaviorState.Strafing)
        {
            /**
             * We check to see if the enemy has strafed long enough, and if so, set its state to attacking
             * and also reset the strafe timer
             */
            if (strafeTime >= strafeTimeThreshold)
            {
                behaviorState = EnemyBehaviorState.MeleeAttacking;
                strafeTime = 0;
            }
            //This else runs when the enemy is still strafing or just about to start strafing
            else
            {
                //If the enemy is about to start strafing, determine whether it will be right or left
                if (behaviorState == EnemyBehaviorState.TrackingPlayer)
                {
                    int strafeRightNum = Random.Range(0, 1);
                    strafeRight = strafeRightNum != 0;
                    behaviorState = EnemyBehaviorState.Strafing;
                }
                //We then begin to strafe and calculate how long we have been strafing
                Strafe(strafeRight);
                strafeTime += Time.deltaTime;
            }
        }
        //If the enemy is ready to attack
        else if (behaviorState == EnemyBehaviorState.MeleeAttacking)
        {
            //If the enemy is far enough away to lunge
            if (Vector3.Distance(transform.position, PlayerController.instance.transform.position) >= meleeLungeDistance)
            {
                LungeAttack();
            }
            //If not, we back up to lunge
            else
            {
                navMeshAgent.updateRotation = false;
                SmallRetreat();
            }
        }
    }
    #endregion
}
